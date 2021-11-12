namespace LiveDraw
{
    public interface ILiveDraw
    {
        public string GetSelectedColor();
       
        void NextColor();

        void PreviousColor();

        void ToggleEngage();

        void Clear();
    }
}