using UnityEngine;
using UnityEngine.UI;

namespace onlinetutorial.controllers
{
    public class info: Popup
    {
        #region SelfVariables
        public static string Infotext="not found";
        [SerializeField] private Text infotext;


        #endregion


        #region UnityMethods

        protected override void ShowBeforeAnimation()
        {
            infotext.text = Infotext;
        }


        #endregion


        #region OtherMethods



        #endregion

    }
}