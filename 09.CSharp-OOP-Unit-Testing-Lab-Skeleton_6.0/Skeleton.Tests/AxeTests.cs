using NUnit.Framework;
using System;

namespace Skeleton.Tests;

[TestFixture]
public class AxeTests
{
    private const int InitialAttackPoints = 10;
    private const int InitialDurabillityPoints = 5;

    private Axe axe;
    private Dummy dummy;

    [SetUp]
    public void SetUp()
    {
        axe = new(InitialAttackPoints, InitialDurabillityPoints);
        dummy = new Dummy(100, 100);
    }

    [Test]
    public void NewAxe_ShouldSetDataCorrectly()
    {
        //Arrange - in Setup new Axe(InitialAttackPoints, InitialDurabillityPoints)

        //Act
        int actualAttackPoints = axe.AttackPoints;
        int actualDurabillityPoints = axe.DurabilityPoints;

        //Assert
        Assert.AreEqual(InitialAttackPoints, actualAttackPoints);
        Assert.AreEqual(InitialDurabillityPoints, actualDurabillityPoints);
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-20)]
    public void Attack_DurabillityPointsNotPositive_ShouldThrowInvalidOperationException(int durabillityPoints)
    {
        //Arrange
        axe = new Axe(InitialAttackPoints, durabillityPoints);

        //Act
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => axe.Attack(dummy));

        //Assert
        Assert.AreEqual("Axe is broken.", exception.Message);
    }

    [TestCase(1, 0)]
    [TestCase(20, 19)]
    public void Attack_ShouldDecreaseDurabillityPoints(int initialDurabillityPoints, int expectedDurabillityPoints)
    {
        //Arrange
        axe = new Axe(InitialAttackPoints, initialDurabillityPoints);

        //Act
        axe.Attack(dummy);

        //Assert
        Assert.AreEqual(expectedDurabillityPoints, axe.DurabilityPoints);
    }
}