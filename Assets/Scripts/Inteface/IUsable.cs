public interface IUsable {
    long Guid { get; }
    bool CanUse { get; }
    void Use(PlayableCharacter C);
}