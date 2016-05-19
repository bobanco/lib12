﻿using System;
using System.Linq;
using lib12.Data.Random;
using Should;
using Xunit;

namespace lib12.Test.Data.Random
{
    public class RandomClassGeneratorTests
    {
        #region Const
        private const int CollectionSize = 12;
        #endregion

        [Fact]
        public void Generate_returns_not_null_items_of_correct_type()
        {
            var generated = Rand.NextArrayOf<Account>(CollectionSize);
            generated.Count().ShouldEqual(CollectionSize);

            foreach (var item in generated)
            {
                item.ShouldNotBeNull();
                item.ShouldBeType<Account>();
            }
        }

        [Fact]
        public void string_generation_test()
        {
            const int minLength = 3;
            const int maxLength = 7;
            var generated = Rand.NextArrayOf(CollectionSize, new StringGenerator<ClassToGenerate>(x => x.Text, minLength, maxLength));

            foreach (var item in generated)
            {
                item.Text.ShouldNotBeEmpty();
                item.Text.Length.ShouldBeInRange(minLength, maxLength);
            }
        }

        [Fact]
        public void enum_generation_test()
        {
            var generated = Rand.NextArrayOf(CollectionSize, new EnumGenerator<ClassToGenerate, ClassToGenerate.EnumToGenerate>(x => x.Enum));

            foreach (var item in generated)
            {
                item.ShouldNotBeNull();
            }
        }

        [Fact]
        public void bool_generation_test()
        {
            var generated = Rand.NextArrayOf(CollectionSize, new BoolGenerator<ClassToGenerate>(x => x.Bool));

            foreach (var item in generated)
            {
                item.ShouldNotBeNull();
            }
        }

        [Fact]
        public void int_generation_test()
        {
            var generated = Rand.NextArrayOf(CollectionSize, new IntGenerator<ClassToGenerate>(x => x.Int, 50, 100));

            foreach (var item in generated)
            {
                item.ShouldNotBeNull();
                item.Int.ShouldBeInRange(50, 100);
            }
        }

        [Fact]
        public void double_generation_test()
        {
            var generated = Rand.NextArrayOf(CollectionSize, new DoubleGenerator<ClassToGenerate>(x => x.Double, 70, 120));

            foreach (var item in generated)
            {
                item.ShouldNotBeNull();
                item.Double.ShouldBeInRange(70, 120);
            }
        }

        [Fact]
        public void creation_of_complex_class_test()
        {
            var generated = Rand.NextArrayOf<Account>(CollectionSize);

            foreach (var item in generated)
            {
                item.Name.ShouldNotBeEmpty();
                item.Email.ShouldContain("@");
                FakeData.Surnames.ShouldContain(item.Surname);
                item.Address.ShouldNotBeEmpty();
                FakeData.Countries.ShouldContain(item.Country);
                FakeData.Companies.ShouldContain(item.Company);
                item.Info.ShouldNotBeEmpty();
                item.Created.ShouldNotEqual(new DateTime());
            }

            generated.Any(x => Math.Abs(x.Number) > double.Epsilon).ShouldBeTrue();
        }

        [Fact]
        public void available_values_generator()
        {
            var names = new[] { "name1", "name2", "name3" };
            var generated = Rand.NextArrayOf(CollectionSize, new AvailableValuesGenerator<Account, string>(x => x.Name, names));

            foreach (var item in generated)
            {
                names.ShouldContain(item.Name);
            }
        }

        [Fact]
        public void private_properties_arent_override()
        {
            var generated = Rand.Next<ClassToGenerate>();
            generated.NumberThatShouldntBeSet.ShouldEqual(12);
        }

        [Fact]
        public void nested_classes_are_generated_properly()
        {
            var generated = Rand.Next<ClassToGenerate>();
            generated.NestedClass.ShouldNotBeNull();
            generated.NestedClass.NestedText.ShouldNotBeEmpty();
        }
    }
}
