using NUnit.Framework;
using System;

namespace Skeleton.Tests;

[TestFixture]
public class DummyTests
{
    private const int InitialHealthPoints = 5;
    private const int InitialExperiencePoints = 2;

    private Dummy dummy;

    [SetUp]
    public void SetUp()
    {
        dummy = new Dummy(InitialHealthPoints, InitialExperiencePoints);
    }

    [Test]
    public void NewDummy_ShouldSetDataCorrectly()
    {
        //Arrange in Setup()

        //Act and Assert
        Assert.AreEqual(InitialHealthPoints, dummy.Health);
    }

    [Test]
    public void TakeAttack_ShouldDecreaseHealth()
    {
        //Arrange
        int takeAttackPoints = 3;
        int expectedAttackPoints = InitialHealthPoints - takeAttackPoints;

        //Act

        dummy.TakeAttack(takeAttackPoints);

        //Assert
        Assert.AreEqual(expectedAttackPoints, dummy.Health);
    }

    [TestCase(5)]
    [TestCase(6)]
    [TestCase(100)]
    public void TakeAttack_WhenIsDead_ShouldThrowError(int takeAttackPoints)
    {
        //Act
        dummy.TakeAttack(takeAttackPoints);

        //Assert
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => dummy.TakeAttack(1));

        Assert.AreEqual("Dummy is dead.", exception.Message);
    }

    [TestCase(5)]
    [TestCase(6)]
    [TestCase(100)]
    public void GiveExperience_WhenIsDead_ShouldReturnCorrectValue(int takeAttackPoints)
    {
        //Act
        dummy.TakeAttack(takeAttackPoints);

        //Assert
        Assert.AreEqual(InitialExperiencePoints, dummy.GiveExperience());
    }

    [Test]
    public void GiveExperience_WhenNotDead_ShouldThrowError()
    {
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => dummy.GiveExperience());

        Assert.AreEqual("Target is not dead.", exception.Message);
    }

    [TestCase(-10, true)]
    [TestCase(-1, true)]
    [TestCase(0, true)]
    [TestCase(1, false)]
    [TestCase(20, false)]
    public void IsDead_ShouldReturnCorrectValue(int healthPoints, bool expectedIsDead)
    {
        dummy = new(healthPoints, InitialExperiencePoints);

        Assert.AreEqual(expectedIsDead, dummy.IsDead());
    }
}