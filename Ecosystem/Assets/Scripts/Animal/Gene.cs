using System;
using UnityEngine;
using UnityEngine.ProBuilder;
using Random = UnityEngine.Random;

namespace Animal
{
  public class Gene
  {
    private const float BaseMax = 1.2f;
    private const float BaseMin = 0.8f;

    public int Bits { get; private set; }
    public float Value { get; private set; }
    public byte Chromosome { get; private set; }

    public Gene()
    {
      Chromosome = (byte) Random.Range(byte.MinValue, byte.MaxValue);
      Bits = CountSetBits(Chromosome);
      Value = EvaluateValue(BaseMax, BaseMin);
    }

    public Gene(Gene father, Gene mother)
    {
      Chromosome = MakeChromosomeUniform(father.Chromosome, mother.Chromosome);
      Bits = CountSetBits(Chromosome);
      Value = father.Value < mother.Value ? EvaluateValue(mother, father) 
                                          : EvaluateValue(father, mother);
    }

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

    private float EvaluateValue(Gene max, Gene min)
    {
      float value;
      var chunk = (max.Value - min.Value) / 7;
      
      if (Bits == max.Bits && Bits == min.Bits) return Random.Range(min.Value, max.Value);
      
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

    private static int CountSetBits(byte chromosome) 
    { 
      var count = 0; 
      while (chromosome > 0) { 
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
        if (Random.Range(0, 1f) < 0.5f)
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

    private static byte MakeChromosomeFitness(Gene father, Gene mother)
    {
      var fitnessRatio = father.Bits / (father.Bits + mother.Bits);
      byte chromosome = 0;
      byte currentBitValue = 1;
      while (father.Chromosome > 0 || mother.Chromosome > 0)
      {
        if (Random.Range(0, 1f) < fitnessRatio)
        {
          var fatherBit = father.Chromosome & 1;
          chromosome += (byte) (currentBitValue * fatherBit);
        }
        else
        {
          var motherBit = mother.Chromosome & 1;
          chromosome += (byte) (currentBitValue * motherBit);
        }
        father.Chromosome >>= 1;
        mother.Chromosome >>= 1;
        currentBitValue *= 2;
      }

      return chromosome;
    }

    public void Mutate()
    {
      if (!(Random.Range(0, 1f) < 0.02f)) return;
      var oldChromosome = Chromosome;
      Chromosome = (byte) Random.Range(byte.MinValue, byte.MaxValue);
      Debug.Log("MUTATED!!!!!!!!!!!!!");
      if (Chromosome > oldChromosome)
      {
        Value *= Random.Range(1, 1.05f);
      } 
      else if (Chromosome < oldChromosome)
      {
        Value *= Random.Range(0.95f, 1);
      }
    }
  }
}