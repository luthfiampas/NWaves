﻿using System;
using System.Linq;
using NWaves.Signals;

namespace NWaves.Operations
{
    /// <summary>
    ///  Class providing modulation methods:
    /// 
    ///     - ring
    ///     - amplitude
    ///     - frequency
    ///     - phase
    /// 
    /// </summary>
    public static class Modulate
    {
        /// <summary>
        /// Ring modulation (RM)
        /// </summary>
        /// <param name="carrier">Carrier signal</param>
        /// <param name="modulator">Modulator signal</param>
        /// <returns>RM signal</returns>
        public static DiscreteSignal Ring(DiscreteSignal carrier,
                                          DiscreteSignal modulator)
        {
            if (carrier.SamplingRate != modulator.SamplingRate)
            {
                throw new ArgumentException("Sampling rates must be the same!");
            }

            return new DiscreteSignal(carrier.SamplingRate,
                                      carrier.Samples.Zip(modulator.Samples, (c, m) => c * m));
        }

        /// <summary>
        /// Amplitude modulation (AM)
        /// </summary>
        /// <param name="carrier">Carrier signal</param>
        /// <param name="modulatorFrequency">Modulator frequency</param>
        /// <param name="modulationIndex">Modulation index (depth)</param>
        /// <returns>AM signal</returns>
        public static DiscreteSignal Amplitude(DiscreteSignal carrier, 
                                               double modulatorFrequency = 20/*Hz*/,
                                               double modulationIndex = 0.5)
        {
            var samplingRate = carrier.SamplingRate;

            var output = Enumerable.Range(0, carrier.Length)
                                   .Select(i => carrier[i] * (1 + modulationIndex * Math.Cos(2 * Math.PI * modulatorFrequency / samplingRate * i)));

            return new DiscreteSignal(samplingRate, output);
        }

        /// <summary>
        /// Frequency modulation (FM)
        /// </summary>
        /// <param name="baseband">Baseband signal</param>
        /// <param name="carrierAmplitude">Carrier amplitude</param>
        /// <param name="carrierFrequency">Carrier frequency</param>
        /// <param name="deviation">Frequency deviation</param>
        /// <returns>RM signal</returns>
        public static DiscreteSignal Frequency(DiscreteSignal baseband,
                                               double carrierAmplitude,
                                               double carrierFrequency,
                                               double deviation = 100/*Hz*/)
        {
            var samplingRate = baseband.SamplingRate;

            var integral = 0.0;
            var output = Enumerable.Range(0, baseband.Length)
                                   .Select(i => carrierAmplitude * Math.Cos(2 * Math.PI * carrierFrequency / samplingRate * i +
                                                2 * Math.PI * deviation * (integral += baseband[i])));

            return new DiscreteSignal(samplingRate, output);
        }

        /// <summary>
        /// Sinusoidal frequency modulation (FM)
        /// </summary>
        /// <param name="carrierFrequency">Carrier signal frequency</param>
        /// <param name="carrierAmplitude">Carrier signal amplitude</param>
        /// <param name="modulatorFrequency">Modulator frequency</param>
        /// <param name="modulationIndex">Modulation index (depth)</param>
        /// <param name="length">Length of FM signal</param>
        /// <param name="samplingRate">Sampling rate</param>
        /// <returns>Sinusoidal FM signal</returns>
        public static DiscreteSignal FrequencySinusoidal(
                                        double carrierFrequency,
                                        double carrierAmplitude,
                                        double modulatorFrequency,
                                        double modulationIndex,
                                        int length,
                                        int samplingRate = 1)
        {
            var output = Enumerable.Range(0, length)
                                   .Select(i => carrierAmplitude * Math.Cos(2 * Math.PI * carrierFrequency / samplingRate * i + 
                                                modulationIndex * Math.Sin(2 * Math.PI * modulatorFrequency / samplingRate * i)));

            return new DiscreteSignal(samplingRate, output);
        }

        /// <summary>
        /// Linear frequency modulation (FM)
        /// </summary>
        /// <param name="carrierFrequency">Carrier signal frequency</param>
        /// <param name="carrierAmplitude">Carrier signal amplitude</param>
        /// <param name="modulationIndex">Modulation index (depth)</param>
        /// <param name="length">Length of FM signal</param>
        /// <param name="samplingRate">Sampling rate</param>
        /// <returns>Sinusoidal FM signal</returns>
        public static DiscreteSignal FrequencyLinear(
                                        double carrierFrequency,
                                        double carrierAmplitude,
                                        double modulationIndex,
                                        int length,
                                        int samplingRate = 1)
        {
            var output = Enumerable.Range(0, length)
                                   .Select(i => carrierAmplitude * Math.Cos(2 * Math.PI * carrierFrequency / samplingRate * i +
                                                (modulationIndex * i * i) / (2 * samplingRate * samplingRate )));

            return new DiscreteSignal(samplingRate, output);
        }

        /// <summary>
        /// Phase modulation (PM)
        /// </summary>
        /// <param name="baseband">Baseband signal</param>
        /// <param name="carrierAmplitude">Carrier amplitude</param>
        /// <param name="carrierFrequency">Carrier frequency</param>
        /// <param name="deviation">Frequency deviation</param>
        /// <returns>RM signal</returns>
        public static DiscreteSignal Phase(DiscreteSignal baseband,
                                           double carrierAmplitude,
                                           double carrierFrequency,
                                           double deviation = 0.5)
        {
            var samplingRate = baseband.SamplingRate;

            var output = Enumerable.Range(0, baseband.Length)
                                   .Select(i => carrierAmplitude * Math.Cos(2 * Math.PI * carrierFrequency / samplingRate * i + 
                                                deviation * baseband[i]));

            return new DiscreteSignal(samplingRate, output);
        }
    }
}