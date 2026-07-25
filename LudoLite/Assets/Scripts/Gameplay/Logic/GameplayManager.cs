using System;
using System.Collections;
using Assets.Scripts.Networking;
using Assets.Scripts.UI.Dice;
using Assets.Scripts.UI.Game;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Logic
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance;

        public int LastDiceRoll { get; private set; }

        public bool NewDiceRollAvailable { get; set; }

        public bool IsChipMoving { get; set; }

        [SerializeField] private LudoBoardUiComponent _ludoBoardUiComponent;

        [SerializeField] private GamePanelUiComponent _gamePanelUiComponent;

        [SerializeField] private DiceAnimate _diceAnimate;

        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AudioClip _diceRollAudio;

        [SerializeField] private AudioClip _chipMoveAudio;

        private bool _isFetchingRandomNumber;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LastDiceRoll = -1;
            ResetPositions(_gamePanelUiComponent.PlayerChips);
        }

        public void RollDice(DiceAnimate diceAnimate)
        {
            if (_isFetchingRandomNumber || IsChipMoving) return;

            diceAnimate.Animate();

            PlayRollAudio();

            _isFetchingRandomNumber = true;
            Action<int> fetchRandomSuccess = (result) =>
            {
                _audioSource.Stop();

                diceAnimate.Stop();
                diceAnimate.SetDiceResult(result);
                _isFetchingRandomNumber = false;
                LastDiceRoll = result;
                NewDiceRollAvailable = true;
            };

            Action<string> fetchRandomError = (message) =>
            {
                _audioSource.Stop();

                diceAnimate.Stop();
                Debug.LogError(message);
                _isFetchingRandomNumber = false;
                NewDiceRollAvailable = false;
            };

            StartCoroutine(RandomUtility.FetchDiceRandomNumber(fetchRandomSuccess, fetchRandomError));
        }

        public void ResetPositions(PlayerChip[] playerChips)
        {
            if (IsChipMoving) return;

           foreach (var chip in playerChips)
           {
               chip.ResetPosition(_ludoBoardUiComponent);
           }

           NewDiceRollAvailable = false;
           _diceAnimate.HideDiceImage();
        }

        public IEnumerator MoveChip(PlayerChip playerChip)
        {
            if (IsChipMoving || NewDiceRollAvailable == false) yield break;

            IsChipMoving = true;
            
            yield return ChipMover.Move(playerChip, _ludoBoardUiComponent);

            NewDiceRollAvailable = false;
            IsChipMoving = false;
            _diceAnimate.HideDiceImage();
        }

        public void PlayRollAudio()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            _audioSource.loop = true;
            _audioSource.clip = _diceRollAudio;
            _audioSource.Play();
        }

        public void PlayChipMoveAudio()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            _audioSource.loop = false;
            _audioSource.clip = _chipMoveAudio;
            _audioSource.Play();
        }
    }
}
