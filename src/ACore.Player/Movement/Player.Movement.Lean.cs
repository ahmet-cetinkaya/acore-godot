using ACore.Player.Movement.constants;
using Godot;

namespace ACore.Player;

public partial class Player : CharacterBody3D
{
    [Export]
    [ExportGroup("Movement/Lean")]
    public bool LeanEnabled { get; set; } = true;

    [Export]
    [ExportGroup("Movement/Lean")]
    public float LeaningDuration { get; set; } = 1f;

    [Export]
    [ExportGroup("Movement/Lean")]
    private ShapeCast3D _leanPivotLeftShapeCast { get; set; }

    [Export]
    [ExportGroup("Movement/Lean")]
    private ShapeCast3D _leanPivotRightShapeCast { get; set; }

    [Export]
    [ExportGroup("Movement/Lean")]
    private Node3D _leanPivot;

    private Tween _leanBlendTween;
    private Tween _leanBlendCollisionTween;

    private const string LEAN_BLEND_AMOUNT_PROPERTY = "parameters/LeanBlend/blend_amount";
    private const string LEAN_BLEND_LEFT_COLLISION_AMOUNT_PROPERTY =
        "parameters/LeanLeftCollisionBlend/blend_amount";
    private const string LEAN_BLEND_RIGHT_COLLISION_AMOUNT_PROPERTY =
        "parameters/LeanRightCollisionBlend/blend_amount";

    private void handleMovementLeanPhysics()
    {
        if (!LeanEnabled)
            return;

        _leanBlendTween?.Kill();

        Variant leanX = 0;

        if (IsOnFloor())
        {
            float leanBlendCollisionValue;
            if (Input.IsActionPressed(PlayerMovementInputControls.LeanLeft))
            {
                leanX = -1;

                _leanBlendCollisionTween?.Kill();
                leanBlendCollisionValue = _leanPivotLeftShapeCast.IsColliding() ? 1 : 0;
                _leanBlendCollisionTween = AnimationTree
                    .GetTree()
                    .CreateTween()
                    .SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Back);
                _ = _leanBlendCollisionTween.TweenProperty(
                    AnimationTree,
                    LEAN_BLEND_LEFT_COLLISION_AMOUNT_PROPERTY,
                    leanBlendCollisionValue,
                    LeaningDuration
                );
            }
            else if (Input.IsActionPressed(PlayerMovementInputControls.LeanRight))
            {
                leanX = 1;

                _leanBlendCollisionTween?.Kill();
                leanBlendCollisionValue = _leanPivotRightShapeCast.IsColliding() ? 1 : 0;
                _leanBlendCollisionTween = AnimationTree
                    .GetTree()
                    .CreateTween()
                    .SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Back);
                _ = _leanBlendCollisionTween.TweenProperty(
                    AnimationTree,
                    LEAN_BLEND_RIGHT_COLLISION_AMOUNT_PROPERTY,
                    leanBlendCollisionValue,
                    LeaningDuration
                );
            }
        }

        _leanBlendTween = AnimationTree
            .GetTree()
            .CreateTween()
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Back);
        _ = _leanBlendTween.TweenProperty(
            AnimationTree,
            LEAN_BLEND_AMOUNT_PROPERTY,
            leanX,
            LeaningDuration
        );
    }
}
