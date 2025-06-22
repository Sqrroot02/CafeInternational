using NUnit.Framework;
using Assets.Scripts.Models;
using UnityEngine;

namespace Tests.ComplexBotBehaviourTest
{
    public class ChairMappingTests
    {
        private TestableChairMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mapper = new TestableChairMapper();
    }

    [Test]
    public void Joker_Should_Contain_All_Chairs()
    {
        // Arrange
        var chair1 = CreateChair(Nationality.France);
        var chair2 = CreateChair(Nationality.German, Nationality.USA);
        _mapper.SetChairs(new[] { chair1, chair2 });

        // Act
        _mapper.InvokeMapNationalityToChairListDictionary();

        // Assert
        Assert.IsTrue(_mapper.Dictionary.ContainsKey(Nationality.Joker));
        CollectionAssert.AreEquivalent(new[] { chair1, chair2 }, _mapper.Dictionary[Nationality.Joker]);
    }

    [Test]
    public void Single_Nationality_Should_Map_Correctly()
    {
        var chair = CreateChair(Nationality.Italy);
        _mapper.SetChairs(new[] { chair });

        _mapper.InvokeMapNationalityToChairListDictionary();

        Assert.IsTrue(_mapper.Dictionary.ContainsKey(Nationality.Italy));
        Assert.Contains(chair, _mapper.Dictionary[Nationality.Italy]);
    }

    [Test]
    public void Dual_Nationality_Should_Map_Both()
    {
        var chair = CreateChair(Nationality.Russia, Nationality.China);
        _mapper.SetChairs(new[] { chair });

        _mapper.InvokeMapNationalityToChairListDictionary();

        Assert.Contains(chair, _mapper.Dictionary[Nationality.Russia]);
        Assert.Contains(chair, _mapper.Dictionary[Nationality.China]);
    }

    [Test]
    public void Duplicate_Chairs_Should_Be_Handled_Gracefully()
    {
        var chair1 = CreateChair(Nationality.Spain);
        var chair2 = CreateChair(Nationality.Spain);
        _mapper.SetChairs(new[] { chair1, chair2 });

        _mapper.InvokeMapNationalityToChairListDictionary();

        Assert.AreEqual(2, _mapper.Dictionary[Nationality.Spain].Count);
    }

    // Helper Methods
    private Chair CreateChair(Nationality nat1, Nationality? nat2 = null)
    {
        var chair = new GameObject().AddComponent<TestChair>();
        var firstTable = new GameObject().AddComponent<TestTable>();
        firstTable.nationality = nat1;
        chair._firstTable = firstTable;
        chair._secondTable = nat2.HasValue ? CreateTable(nat2.Value) : null;
        return chair;
    }

    private Table CreateTable(Nationality nationality)
    {
        var table = new GameObject().AddComponent<TestTable>();
        table.nationality = nationality;
        return table;
    }
    }
}