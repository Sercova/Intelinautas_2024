using System.Collections.Generic;
using IntelliChallenge.RavenMatrix.ChallengeFormatter;
using MiscUtil.Collections.Extensions;
using Unity.VisualScripting.ReorderableList;

namespace IntelliChallenge.RavenMatrix
{
    public class ChallengeBuilder
    {
       // public List<ChallengeElement> elementList;

        public List<ChallengeElement> respuestaElementList;
    //    private List<ChallengeItemFormat> formatList;
       // public ChallengeFormat format { get; set; }
       public ChallengeBuilder()
       {
           
       }
        public List<ChallengeElement> Construir(ChallengeFormat format)
        {
            List<ChallengeElement> elementList = new List<ChallengeElement>();
            for (int i = 0; i < format.elementsQty; i++)
            {
                ChallengeElement element = new ChallengeElement();
                //está definido un formato o elemento o es progresion?
                ChallengeElementFormat elementFormat = format.ElementFormatsList[0];
                if (format.ElementFormatsList.Count == format.elementsQty)
                {
                    elementFormat = format.ElementFormatsList[i];
                }
                for (int j = 0; j < elementFormat.itemsQty; j++)
                {
                    ChallengeItemFormat itemFormat = elementFormat.ItemFormatsList[j];
                    ChallengeItem item = new ChallengeItem();
                    item.type = itemFormat.itemType;
                    foreach (var itemFormatBehavior in itemFormat.Behaviors)
                    {
                     //   var valor = itemFormat.item.valorInicial + itemFormat.incremento * i;
                        var valor = itemFormatBehavior.initialValue + itemFormatBehavior.increment * i;
                        switch (itemFormatBehavior.type)
                        {
                            case BehaviorType.Sides:
                                item.sidesNumber = (int)valor;
                                break;
                            case BehaviorType.Radius:
                                item.radius = valor;
                                break;
                            case BehaviorType.Position:
                                item.LocalPosition = valor;
                                break;
                            case BehaviorType.Spin:
                                item.spin = valor;
                                break;
                            case BehaviorType.isOn:
                                item.isOn = valor;
                                break;
                        }
                    }
                    element.itemsList.Add(item);
                }
                elementList.Add(element);
            }

            return elementList;

        }
    }
}