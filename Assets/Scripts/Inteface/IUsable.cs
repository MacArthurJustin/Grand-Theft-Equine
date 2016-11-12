public interface IUsable {
    int Guid { get; }
    bool CanUse { get; }
    void Use(PlayableCharacter C);
}