using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Int32;
using Microsoft.Extensions.Logging;
using Peintures.Core.Entities;

namespace Peintures.Data
{
    public class PeinturesCsvFileStorage : IPeinturesStorage
    {
        private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;

        private readonly Dictionary<string, Owner> owners = new Dictionary<string, Owner>(collation);
        private readonly Dictionary<string, Artist> artists = new Dictionary<string,Artist>(collation);
        private readonly Dictionary<string, Painting> paintings = new Dictionary<string, Painting>(collation);
        private readonly ILogger<PeinturesCsvFileStorage> logger;
        private IPeinturesStorage _peinturesStorageImplementation;

        public PeinturesCsvFileStorage(ILogger<PeinturesCsvFileStorage> logger)
        {
            this.logger = logger;
            ReadOwnersFromCsvFile("owners.csv");
            ReadArtistsFromCsvFile("artists.csv");
            ReadPaintingsFromCsvFile("paintings.csv");
            ResolveReferences();
        }

        private void ResolveReferences()
        {
            

            foreach (var art in artists.Values)
            {
                art.Paintings = paintings.Values.Where(p => p.Artist == art.FullNameArtist).ToList();
                foreach (var painting in art.Paintings) painting.ArtistName = art;
            }
            foreach (var ow in owners.Values)
            {
                ow.Paintings = paintings.Values.Where(p => p.Owner == ow.NameOwner).ToList();
                foreach (var painting in ow.Paintings) painting.OwnerName = ow;
            }
        }

        private string ResolveCsvFilePath(string filename)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csvFilePath = Path.Combine(directory, "csv-data");
            return Path.Combine(csvFilePath, filename);
        }

        private void ReadPaintingsFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var painting = new Painting
                {
                    Name = tokens[0],
                    Artist = tokens[1],
                    Owner = tokens[2],
                    Genre = tokens[4],
                };
                if (TryParse(tokens[3], out var yearOfPainting)) painting.YearOfPainting = yearOfPainting;
                paintings[painting.Name] = painting;
            }

            logger.LogInformation($"Loaded {paintings.Count} paintings from {filePath}");
        }

        private void ReadArtistsFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var artist = new Artist
                {
                    FullNameArtist = tokens[0],
                    YearsOfLife = tokens[1],
                };
                artists.Add(artist.FullNameArtist, artist);
            }

            logger.LogInformation($"Loaded {artists.Count} artists from {filePath}");
        }

        private void ReadOwnersFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var owner = new Owner
                {
                    NameOwner = tokens[0],
                    Type = tokens[1],
                    Location = tokens[2]
                };
                owners.Add(owner.NameOwner, owner);
            }

            logger.LogInformation($"Loaded {owners.Count} owners from {filePath}");
        }

        public int CountPaintings() => paintings.Count;

        public IEnumerable<Painting> ListPaintings() => paintings.Values;

        public IEnumerable<Owner> ListOwners() => owners.Values;

        public IEnumerable<Artist> ListArtists() => artists.Values;

        public Painting FindPainting(string name) => paintings.GetValueOrDefault(name);

        public Artist FindArtist(string fullNameArtist) => artists.GetValueOrDefault(fullNameArtist);

        public Owner FindOwner(string nameOwner) => owners.GetValueOrDefault(nameOwner);

        public void CreatePainting(Painting painting)
        {
            painting.ArtistName.Paintings.Add(painting);
            painting.Artist = painting.ArtistName.FullNameArtist;
            painting.OwnerName.Paintings.Add(painting);
            painting.Owner = painting.OwnerName.NameOwner;
            UpdatePainting(painting);
        }

        public void UpdatePainting(Painting painting)
        {
            paintings[painting.Name] = painting;
        }

        public void DeletePainting(Painting painting)
        {
            var artist = FindArtist(painting.Artist);
            artist.Paintings.Remove(painting);
            var owner = FindOwner(painting.Owner);
            owner.Paintings.Remove(painting);
            paintings.Remove(painting.Name);
        }
    }
}