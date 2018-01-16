﻿using PokemonGenerator.Enumerations;
using PokemonGenerator.Validators;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace PokemonGenerator.Tests.Validator_Tests
{
    public class PokeGeneratorOptionsValidatorTests
    {
        private readonly IPokeGeneratorOptionsValidator _validator;

        public PokeGeneratorOptionsValidatorTests()
        {
            _validator = new PokeGeneratorOptionsValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ThisNameIsFarTooLong")]
        [InlineData("         ")]
        [InlineData("123456789")]
        [InlineData("Player_1")]
        [InlineData("Player 1")]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateNameFailsTest(string name)
        {
            Assert.False(_validator.ValidateName(name));
        }

        [Theory]
        [InlineData("Player1")]
        [InlineData("P")]
        [InlineData("1")]
        [InlineData("12345678")]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateNameSucceedsTest(string name)
        {
            Assert.True(_validator.ValidateName(name));
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(101)]
        [InlineData(4)]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateLevelFailsTest(int level)
        {
            Assert.False(_validator.ValidateLevel(level));
        }

        [Theory]
        [InlineData(5)]
        [InlineData(100)]
        [InlineData(99)]
        [InlineData(6)]
        [InlineData(50)]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateLevelSucceedsTest(int level)
        {
            Assert.True(_validator.ValidateLevel(level));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateEntropySucceedsWithEnumValsTest()
        {
            foreach (var name in Enum.GetNames(typeof(Entropy)))
            {
                Assert.True(_validator.ValidateEntropy(name));
            }
        }

        [Theory]
        [InlineData("Fake")]
        [InlineData("LOW")]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateEntropyFailsTest(string entropy)
        {
            Assert.False(_validator.ValidateEntropy(entropy));
        }

        [Theory]
        [InlineData("Gold")]
        [InlineData("Silver")]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateGameSucceedsTest(string game)
        {
            Assert.True(_validator.ValidateGame(game));
        }

        [Theory]
        [InlineData("Ruby")]
        [InlineData("Emerald")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateGameFailsTest(string game)
        {
            Assert.False(_validator.ValidateGame(game));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("\t     \n\r")]
        [InlineData("/Fakey/Fake/Dir/")]
        [InlineData(@"C:\%^&#%$&((&#$\")]
        [InlineData(@"myFile.txt")]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateFileOptionFailsTest(string path)
        {
            Assert.False(_validator.ValidateFileOption(path, ".sav"));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateFileOptionFailsDueToExtensionTest()
        {
            var actualFile = Assembly.GetExecutingAssembly().Location;
            Assert.False(_validator.ValidateFileOption(actualFile, ".fake"));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateFileOptionSucceedsTest()
        {
            var actualFile = Assembly.GetExecutingAssembly().Location;
            Assert.True(_validator.ValidateFileOption(actualFile, ".dll"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("\t     \n\r")]
        [InlineData(@"C:\<>:""/\|?*")]
        [Trait("Category", "Integration")]
        public void PokeGeneratorOptionsValidateFilePathOptionFailsTest(string path)
        {
            Assert.False(_validator.ValidateFilePathOption(path, ".sav"));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateFilePathOptionFailsDueToExtensionTest()
        {
            var actualFile = Assembly.GetExecutingAssembly().Location;
            Assert.False(_validator.ValidateFilePathOption(actualFile, ".fake"));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateFilePathOptionSucceedsTest()
        {
            var actualFile = Assembly.GetExecutingAssembly().Location;
            Assert.True(_validator.ValidateFilePathOption(actualFile, ".dll"));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateFilePathOptionDoesNotxistSucceedsTest()
        {
            var actualFile = Assembly.GetExecutingAssembly().Location;
            var couldBeAFile = Path.Combine(Path.GetDirectoryName(actualFile), "Fake/FakeyFakeTown/Test", Path.GetFileName(actualFile));
            Assert.True(_validator.ValidateFilePathOption(actualFile, ".dll"));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData("  ", "       ")]
        [InlineData("  ", @"C:\ActualFile.txt")]
        [InlineData(@"C:\FakeFile.txt", @"C:\FakeFile.txt")]
        [InlineData(@"C:\FakeFILE.txt", @"C:\FakeFile.txt")]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateUniquePathFailsTest(string path1, string path2)
        {
            Assert.False(_validator.ValidateUniquePath(path1, path2));
        }

        [Theory]
        [InlineData(@"C:\ActualFile.txt", @"C:\ActualFile2.txt")]
        [InlineData(@"C:\One\FakeFile.txt", @"C:\Two\FakeFile.txt")]
        [Trait("Category", "Unit")]
        public void PokeGeneratorOptionsValidateUniquePathSucceedsTest(string path1, string path2)
        {
            Assert.True(_validator.ValidateUniquePath(path1, path2));
        }
    }
}