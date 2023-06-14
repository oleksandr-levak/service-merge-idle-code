using System;
using System.Collections.Generic;
using MergeIdle.Scripts.Managers.Tutorial.Enum;
using MergeIdle.Scripts.Managers.Tutorial.Views;
using MergeIdle.Scripts.Storage.Enum;
using UnityEngine;

namespace MergeIdle.Scripts.Managers.Tutorial.Controllers
{
    public class TutorialManager : MonoBehaviour 
    {
        [SerializeField] private List<FingerView> _fingers;

        public void ShowFinger(EFinger finger)
        {
            var fingerViews = GetFingers(finger);
            foreach (var fingerView in fingerViews)
            {
                fingerView.Show();
            }
        }
        
        public void HideFinger(EFinger finger)
        {
            var fingerViews = GetFingers(finger);
            foreach (var fingerView in fingerViews)
            {
                fingerView.Hide();  
            }
        }
        
        public EFinger GetFingerByStage(ETutorialStage stage)
        {
            EFinger fingerType = EFinger.MERGE;
            switch (stage)
            {
                case ETutorialStage.PURCHASE:
                {
                    fingerType = EFinger.SMALL_PURCHASE;
                }
                 break;
                case ETutorialStage.TIMER:
                {
                    fingerType = EFinger.SMALL_TIMER;
                }
                    break;
                case ETutorialStage.COMPLETED_ORDER:
                {
                    fingerType = EFinger.COMPLETED_SMALL_ORDERS;
                }
                    break;
                case ETutorialStage.SHOW_ORDER:
                {
                    fingerType = EFinger.SHOW_SMALL_ORDERS;
                }
                    break;
                case ETutorialStage.GENERATOR:
                {
                    fingerType = EFinger.GENERATOR;
                }
                    break;
                case ETutorialStage.UPDATE_SALON:
                {
                    fingerType = EFinger.UPDATE_LABEL;
                }
                    break;
            }

            return fingerType;
        }

        public List<FingerView> GetFingers(EFinger finger) =>
            _fingers.FindAll(x => x.Type == finger);
    }
}