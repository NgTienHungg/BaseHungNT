#if HUNGNT_SPINE
using Spine.Unity;

public static class SkeletonExtensions
{
    public static void Fade(this SkeletonGraphic skeleton, float alpha)
    {
        skeleton.color = skeleton.color.Fade(alpha);
    }

    public static void ResetAnim(this SkeletonAnimation skeleton)
    {
        skeleton.AnimationState.ClearTracks();
        skeleton.Skeleton.SetSlotsToSetupPose();
    }

    public static void ResetAnim(this SkeletonGraphic skeleton)
    {
        skeleton.AnimationState.ClearTracks();
        skeleton.Skeleton.SetSlotsToSetupPose();
    }

    public static void ChangeDataAsset(this SkeletonAnimation skeleton, SkeletonDataAsset skeletonDataAsset)
    {
        skeleton.AnimationState.ClearTracks();
        skeleton.skeletonDataAsset = skeletonDataAsset;
        skeleton.Initialize(overwrite: true);
    }

    public static void ChangeDataAsset(this SkeletonGraphic skeleton, SkeletonDataAsset skeletonDataAsset)
    {
        skeleton.AnimationState.ClearTracks();
        skeleton.skeletonDataAsset = skeletonDataAsset;
        skeleton.Initialize(overwrite: true);
    }
}

#endif