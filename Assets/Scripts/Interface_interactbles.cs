
namespace YahiaDroubiTask
{

    public interface Interface_interactbles
    {
                                                                                                             //out refrence parameter
        //public bool Solu_Interact(float AmountDrpNeeds, float amountDrpHave, UnityEngine.Color? BlendColor,BlendType blendType , out bool isDropperAbsorbin);
        public bool Solu_Interact(float AmountDrpNeeds, float amountDrpHave, BlendInfo blendInfo , out bool isDropperAbsorbin);


        public UnityEngine.GameObject GetBlend();


    }
}