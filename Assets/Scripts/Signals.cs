using deVoid.Utils;
public class ShowDialogueMessageSignal : ASignal<string, PestoEmote, bool> { }
public class EndPauseSignal : ASignal { }
public class HitByBulletSignal : ASignal<EntityType, EntityType, bool> { }
