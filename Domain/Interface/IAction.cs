﻿namespace Domain.Interface
{
    public interface IAction
    {
        void Start();
        void Stop();
        string GetConfiguration();
        string GetActionName();
    }
}
