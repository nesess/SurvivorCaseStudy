namespace Game_Essentials.Event_Bus
{
    public delegate void EventListener<in TEvent>(object sender, TEvent e);
}