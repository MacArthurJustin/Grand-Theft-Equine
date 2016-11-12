public interface IInteractable {
    bool CanInteract { get; }
    void Interact(PlayableCharacter PC);
}
