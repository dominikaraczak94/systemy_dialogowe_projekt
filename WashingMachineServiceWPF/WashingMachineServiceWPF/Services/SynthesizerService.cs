using System.Speech.Synthesis;

namespace WashingMachineServiceWPF.Services
{
    public class SynthesizerService
    {
        public void Synthesize(string text)
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                synth.SelectVoice("Microsoft Paulina Desktop");

                synth.Speak(text);
            }
        }
    }
}