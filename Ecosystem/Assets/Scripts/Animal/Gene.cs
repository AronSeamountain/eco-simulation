using UnityEngine;

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
      Chromosome = MakeChromosomeCrossover(father, mother);
      Bits = CountSetBits(Chromosome);
      Value = father.Value < mother.Value
        ? EvaluateValue4(mother, father)
        : EvaluateValue4(father, mother);
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
      var chunk = (max.Value - min.Value) / 7;

      switch (Bits)
      {
        case 8:
          value = Random.Range(BaseMax, BaseMax + chunk);
          break;
        case 0:
          value = Random.Range(BaseMin - chunk, BaseMin);
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
      var chunk = (max.Value - min.Value) / 7;

      switch (Bits)
      {
        case 8:
          value = Random.Range(max.Value, max.Value + chunk);
          break;
        case 0:
          value = Random.Range(min.Value - chunk, min.Value);
          break;
        default:
          //for example setBits = 1 and min = 0.8 we get random(0.8, 0.857)
          //setBits = 2 -> random(0.857, 0.914)
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

      if (value < 0.2f) value = 0.2f;
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
    ///   takes the bit-ratio between the parents. the parent with more bits have a higher chance to pass on their bits
    ///   to the child's bit string
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