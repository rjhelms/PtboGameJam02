public class StaticEntity : BaseEntity {
    protected override void Start ()
    {
        base.Start();
        base.Update();
    }

    protected override void Update () {}
}