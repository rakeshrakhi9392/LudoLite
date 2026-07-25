using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.AddressablesHelpers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Dice
{
    public class DiceAnimate : MonoBehaviour
    {
        public bool IsAnimating => _animateRoutine != null;

        [SerializeField] private string[] _frameAddresses;

        private List<Sprite> _frames;
        private Coroutine _animateRoutine;

        private void Start()
        {
            _frames = new List<Sprite>();

            foreach (var spriteAddress in _frameAddresses)
            {
                AddressableSpriteLoader.LoadSprite(spriteAddress, (sprite) =>
                {
                    _frames.Add(sprite);
                });
            }
        }

        public void Animate()
        {
            GetComponent<Image>().enabled = true;

            if (IsAnimating)
            {
                Stop();
            }

            _animateRoutine = StartCoroutine(AnimateInternal());
        }

        public void Stop()
        {
            if (_animateRoutine == null) return;

            StopCoroutine(_animateRoutine);
            GetComponent<RectTransform>().rotation = Quaternion.identity;
            _animateRoutine = null;
        }

        public void SetDiceResult(int result)
        {
            if (result != -1)
            {
                GetComponent<Image>().sprite = _frames[Math.Clamp(result - 1, 0, _frames.Count)];
            }
            else
            {
                HideDiceImage();
            }
        }

        public void HideDiceImage()
        {
            GetComponent<Image>().enabled = false;
        }

        private IEnumerator AnimateInternal()
        {
            while (true)
            {
                var rand = new System.Random((int)(Time.time * 1000));
                var frameIndex = rand.Next(0, _frames.Count);
                var rotation = new Vector3(0, 0, rand.Next(-180, 180));

                GetComponent<Image>().sprite = _frames[frameIndex];
                GetComponent<RectTransform>().Rotate(rotation);
               yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
