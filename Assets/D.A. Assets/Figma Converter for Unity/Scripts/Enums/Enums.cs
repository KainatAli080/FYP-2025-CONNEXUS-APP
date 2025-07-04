﻿namespace DA_Assets.FCU
{
    public enum FcuLogType
    {
        Default,
        SetTag,
        IsDownloadable,
        Transform,
        Error,
        GameObjectDrawer,
        HashGenerator
    }

    public enum UIFramework
    {
        UGUI = 0,
        UITK = 1
    }

    public enum ImageFormat
    {
        PNG = 0,
        JPG = 1
    }

    public enum ImageComponent
    {
        UnityImage = 0,
        Shape = 1,
        MPImage = 2,
        ProceduralImage = 3
    }

    public enum TextComponent
    {
        UnityText = 0,
        TextMeshPro = 1
    }

    public enum ShadowComponent
    {
        Figma = 0,
        TrueShadow = 1
    }

    public enum ButtonComponent
    {
        UnityButton = 0,
        DAButton = 1,
    }
}