// namespace WingsMob
// {
//     public partial class UIPopUpManager
//     {
//         public bool HasAnyPopupShowing()
//         {
//             return HasAnyPopupShowingInBoardAds() || HasAnyPopupShowingInBoardFull();
//         }

//         public bool HasAnyPopupShowingInBoardAds()
//         {
//             var count = 0;
//             for (var i = 0; i < m_tranParent.childCount; i++)
//             {
//                 if (m_tranParent.GetChild(i).gameObject.activeSelf)
//                     count++;
//            } 
//             return count > 0;
//         }

//         public bool HasAnyPopupShowingInBoardFull()
//         {
//             var count = 0;
//             for (var i = 0; i < m_tranBoardFull.childCount; i++)
//             {
//                 if (m_tranBoardFull.GetChild(i).gameObject.activeSelf)
//                     count++;
//             }

//             return count > 0;
//         }
//     }
// }