namespace Keys
{
    public struct RoomData
    {
        public RoomData(int _id, int _capacity)
        {
            Id = _id;
            Capacity = _capacity;
        }

        public int Id { get; set; }
        public int Capacity { get; set; }
    }
}