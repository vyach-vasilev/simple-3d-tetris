﻿using System.Collections.Generic;
using UnityEngine;

namespace Octamino
{
    public class KeyboardInput : IPlayerInput
    {
        private KeyCode _pressedKey = KeyCode.None;
        private float _nextRepeatedKeyTime;

        private readonly Dictionary<KeyCode, PlayerAction> _actionForKey = new Dictionary<KeyCode, PlayerAction>
        {
            {KeyCode.A, PlayerAction.MoveLeft},
            {KeyCode.D, PlayerAction.MoveRight},
            {KeyCode.S, PlayerAction.MoveDown},
            {KeyCode.W, PlayerAction.RotateRight},
            {KeyCode.Space, PlayerAction.Fall}
        };

        private readonly List<KeyCode> _repeatingKeys = new List<KeyCode>
        {
            KeyCode.A, KeyCode.D, KeyCode.S
        };

        public PlayerAction? GetPlayerAction()
        {
            var actionKeyDown = GetActionKeyDown();
            if (actionKeyDown != KeyCode.None)
            {
                StartKeyRepeatIfPossible(actionKeyDown);
                return _actionForKey[actionKeyDown];
            }

            if (Input.GetKeyUp(_pressedKey))
            {
                Cancel();
            }
            else
            {
                return GetActionForRepeatedKey();
            }

            return null;
        }

        public void Update()
        {
        }

        public void Cancel()
        {
            _pressedKey = KeyCode.None;
        }

        private void StartKeyRepeatIfPossible(KeyCode key)
        {
            if (_repeatingKeys.Contains(key))
            {
                _pressedKey = key;
                _nextRepeatedKeyTime = Time.time + Constant.Input.KeyRepeatDelay;
            }
        }

        private KeyCode GetActionKeyDown()
        {
            foreach (var key in _actionForKey.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    return key;
                }
            }

            return KeyCode.None;
        }

        private PlayerAction? GetActionForRepeatedKey()
        {
            if (_pressedKey != KeyCode.None && Time.time >= _nextRepeatedKeyTime)
            {
                _nextRepeatedKeyTime = Time.time + Constant.Input.KeyRepeatInterval;
                return _actionForKey[_pressedKey];
            }

            return null;
        }
    }
}