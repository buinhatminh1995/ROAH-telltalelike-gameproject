using System;

public interface ICanClickToActivate{
    void HandleClickToActivate();

    event Action OnOpen;
    event Action OnClose;
}
