using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using highrisehavoc.Source.Entities;
using highrisehavoc.Source.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace highrisehavoc.Source.Controllers
{
    public class SoundController
    {
        private SoundEffect _soundEffectLasershot;
        private SoundEffect _soundEffectLasershot2;
        private SoundEffect _soundEffectARSHOT;

        public bool canPlaySound = true;

        private readonly ContentManager _contentManager;

        public SoundController(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void LoadContent()
        {
            _soundEffectLasershot = _contentManager.Load<SoundEffect>("space-laser-38082-echo");
            _soundEffectLasershot2 = _contentManager.Load<SoundEffect>("space-laser-38082-short");
            _soundEffectARSHOT = _contentManager.Load<SoundEffect>("ar10");
        }

        public void StopSound()
        {
            canPlaySound = false;
        }

        public void EnableSound()
        {
            canPlaySound = true;
        }

        public void playSoundEffectLasershot()
        {
            Console.WriteLine("Can be played: " + canPlaySound.ToString() + "");
            if(canPlaySound) _soundEffectLasershot.Play();
        }

        public void playSoundEffectLasershot2()
        {
            Console.WriteLine("Can be played: " + canPlaySound.ToString() + "");
            if (canPlaySound) _soundEffectLasershot2.Play();
        }

        public void playSoundEffectARSHOT()
        {
            Console.WriteLine("Can be played: " + canPlaySound.ToString() + "");
            if (canPlaySound) _soundEffectARSHOT.Play();
        }

    }
}
