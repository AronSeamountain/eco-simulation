using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animal
{
  public class Gene
  {
    private const float BaseMax = 1.6f;
    private const float BaseMin = 0.8f;

    public Gene()
    {
      Chromosome = (byte) Random.Range(byte.MinValue, byte.MaxValue);
      Bits = CountSetBits(Chromosome);
      Value = EvaluateValue(BaseMax, BaseMin);
    }

    public Gene(Gene father, Gene mother)
    {
      //Chromosome = MakeChromosomeUniform(father.Chromosome, mother.Chromosome);
      Chromosome = MakeChromosomeUniform(father.Chromosome, mother.Chromosome);
      Bits = CountSetBits(Chromosome);
      Value = father.Value < mother.Value
        ? EvaluateValue2(mother, father)
        : EvaluateValue2(father, mother);
    }

    public int Bits { get; }
    public float Value { get; private set; }
    public byte Chromosome { get; private set; }

    public float ChosenParent { get; private set; }

    private float EvaluateValue(float max, float min)
    {
      float value;
      var chunk = (max - min) / 7;

      switch (Bits)
      {
        case 8:
          value = Random.Range(max, max + chunk);
          break;
        case 0:
          value = Random.Range(min - chunk, min);
          break;
        default:
          //for example setBits = 1 and min = 0.8 we get random(0.8, 0.857)
          //setBits = 2 -> random(0.857, 0.914)
          value = Random.Range(min + (Bits - 1) * chunk, min + Bits * chunk);
          break;
      }

      return value;
    }

    /// <summary>
    ///   this evaluates to a value between BaseMin and BaseMax, where BaseMin/BaseMax can mirror the real life values
    /// </summary>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <returns>value of the gene, say speed</returns>
    private float EvaluateValue(Gene max, Gene min)
    {
      float value;
      var chunk = (max.Value - min.Value) / 5;

      switch (Bits)
      {
        case 8:
          value = Random.Range(BaseMax + chunk, BaseMax + 2 * chunk);
          break;
        case 7:
          value = Random.Range(BaseMax, BaseMax + chunk);
          break;
        case 1:
          value = Random.Range(BaseMin - chunk, BaseMin);
          break;
        case 0:
          value = Random.Range(BaseMin - 2 * chunk, BaseMin - chunk);
          break;
        default:
          //for example setBits = 1 and min = 0.8 we get random(0.8, 0.857)
          //setBits = 2 -> random(0.857, 0.914)
          value = Random.Range(BaseMin + (Bits - 1) * chunk, BaseMin + Bits * chunk);
          break;
      }

      return value;
    }

    /// <summary>
    ///   this evaluates to a value between the parents' values
    /// </summary>
    /// <param name="max"></param>
    /// <param name="min"></param>
    /// <returns>value of the gene, say speed</returns>
    private float EvaluateValue2(Gene max, Gene min)
    {
      if (Bits == max.Bits && Bits == min.Bits) return max.Value;

      float value;
      var chunk = (max.Value - min.Value) / 5;

      switch (Bits)
      {
        case 8:
          value = Random.Range(max.Value + chunk, max.Value + 2 * chunk);
          break;
        case 7:
          value = Random.Range(max.Value, max.Value + chunk);
          break;
        case 1:
          value = Random.Range(min.Value - chunk, min.Value);
          break;
        case 0:
          value = Random.Range(min.Value - 2 * chunk, min.Value - chunk);
          break;
        default:
          //for example setBits = 2 and min = 0.8 we get random(0.8, 0.88)
          //setBits = 3 -> random(0.88, 0.96)
          value = Random.Range(min.Value + (Bits - 1) * chunk, min.Value + Bits * chunk);
          break;
      }

      return value;
    }

    private float EvaluateValue3(Gene max, Gene min)
    {
      var number = 0.05f * Bits - 0.05f * (8 - Bits);
      float value = 0;

      if (max.Value >= 1 && min.Value >= 1)
        value = max.Value + number;
      else if (max.Value >= 1 && min.Value <= 1)
        value = ChosenParent + number;
      else if (max.Value <= 1 && min.Value <= 1) value = min.Value + number;
      if (value < 0.2f) value = 0.2f;
      return value;
    }

    private float EvaluateValue4(Gene max, Gene min)
    {
      var number = 0.05f * Bits - 0.05f * (8 - Bits);

      var value = ChosenParent + number;

      if (value < 0.6f) value = 0.6f;
      if (value > 2.2f) value = 2.2f;
      return value;
    }

    private static int CountSetBits(byte chromosome)
    {
      var count = 0;
      while (chromosome > 0)
      {
        count += chromosome & 1;
        chromosome >>= 1;
      }

      return count;
    }


    /// <summary>
    /// Creates a chromosome string from the child's parents' strings.
    /// For each bit in the string there's a 50 percent chance for it to choose either parent.
    /// </summary>
    /// <param name="father"></param>
    /// <param name="mother"></param>
    /// <returns>the bit string of the child</returns>
    private static byte MakeChromosomeUniform(byte father, byte mother)
    {
      byte chromosome = 0;
      byte currentBitValue = 1;
      while (father > 0 || mother > 0)
      {
        if (Random.Range(0, 1f) <= 0.5f)
        {
          var fatherBit = father & 1;
          chromosome += (byte) (currentBitValue * fatherBit);
        }
        else
        {
          var motherBit = mother & 1;
          chromosome += (byte) (currentBitValue * motherBit);
        }

        father >>= 1;
        mother >>= 1;
        currentBitValue *= 2;
      }

      return chromosome;
    }

    /// <summary>
    /// Works similarly to MakeChromosomeUniform.
    /// However, it takes the bit-ratio between the parents.
    /// The parent with more bits have a higher chance to pass on their bits
    /// to the child's bit string.
    /// </summary>
    /// <param name="father"></param>
    /// <param name="mother"></param>
    /// <returns>the bit string of the child</returns>
    private static byte MakeChromosomeFitness(Gene father, Gene mother)
    {
      var fitnessRatio = father.Bits / (father.Bits + mother.Bits);
      byte chromosome = 0;
      byte currentBitValue = 1;
      var dad = father.Chromosome;
      var mom = mother.Chromosome;
      while (dad > 0 || mom > 0)
      {
        if (Random.Range(0, 1f) <= fitnessRatio)
        {
          var fatherBit = dad & 1;
          chromosome += (byte) (currentBitValue * fatherBit);
        }
        else
        {
          var motherBit = mom & 1;
          chromosome += (byte) (currentBitValue * motherBit);
        }

        dad >>= 1;
        mom >>= 1;
        currentBitValue *= 2;
      }

      return chromosome;
    }

    /// <summary>
    /// Crossover principle is used. Meaning that it randomly chooses 1-7 bits of both the
    /// mother's and father's string. This is done by shifting down and then back up again crossoverBits times.
    /// Then it also calculates the remainder of both strings and adds the remainder to the other parent's crossover.
    /// Say we have the father's string XY and mother's AB, where X and A are crossover and Y and B are remainders.
    /// Then we get XB and AY as potential chromosomes for the child, then either XB or AY is chosen by random.
    /// </summary>
    /// <param name="father"></param>
    /// <param name="mother"></param>
    /// <returns>the bit string of the child</returns>
    public byte MakeChromosomeCrossover(Gene father, Gene mother)
    {
      var crossoverBits = Random.Range(1, 8);
      var dad = father.Chromosome;
      var mom = mother.Chromosome;
      var dadCrossover = (byte) ((dad >> crossoverBits) << crossoverBits);
      var momCrossover = (byte) ((mom >> crossoverBits) << crossoverBits);
      var dadRemainder = (byte) (dad - dadCrossover);
      var momRemainder = (byte) (mom - momCrossover);

      var chromosome1 = (byte) (dadCrossover + momRemainder);
      var chromosome2 = (byte) (momCrossover + dadRemainder);


      if (Random.Range(0, 2) == 0)
      {
        ChosenParent = father.Value;
        return chromosome1;
      }

      ChosenParent = mother.Value;
      return chromosome2;
    }

    /// <summary>
    /// Mutates with a 2% chance. Renews the chromosome completely, so it can take on a value
    /// between 0-255, or 0 1's - 8 1's. If the new chromosome is greater/lesser than the old it
    /// increases/decreases the value with 5% 
    /// </summary>
    /// <returns></returns>
    public bool Mutate()
    {
      if (!(Random.Range(0, 1f) < 0.02f)) return false;
      var oldChromosome = Chromosome;
      Chromosome = (byte) Random.Range(byte.MinValue, byte.MaxValue);

      if (Chromosome > oldChromosome)
        Value *= Random.Range(1, 1.05f);
      else if (Chromosome < oldChromosome) Value *= Random.Range(0.95f, 1);

      return true;
    }
  }
}