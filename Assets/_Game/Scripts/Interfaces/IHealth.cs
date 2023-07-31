namespace onlinetutorial.interfaces
{
    public interface IHealth
    {
      
        public void TakeDamage(float damage= 100);
        
        public void TakeHeal(float Heal= 100);


        public float GetHealth();
    }
}