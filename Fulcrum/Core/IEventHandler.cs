namespace Fulcrum.Core
{
	public interface IEventHandler<in TEvent> : IEventHandler
		where TEvent : IEvent
	{
        void Handle(TEvent ev);
	}

	public interface IEventHandler { }
}
