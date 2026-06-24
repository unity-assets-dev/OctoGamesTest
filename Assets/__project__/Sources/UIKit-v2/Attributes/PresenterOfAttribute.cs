using System;

public interface IPresenter {
    void OnEnter(IStateRouter router,IStateScreen screen);
    void BeforeExit(IStateScreen screen);
}

public class PresenterOfAttribute: Attribute {
    public Type[] Types { get; private set; }

    public PresenterOfAttribute(params Type[] presenterType) => Types = presenterType;
}
