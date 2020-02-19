using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatEffect : MonoBehaviour
{
    #region Fields

    private int _Step = 0;
    private bool _CanEffect = false;

    private float _WhiteoutSeconds = 0.5F;
    private float _BreakSeconds = 1.0F;

    private float _Effect1Seconds = 0.0F;
    private float _Effect2Seconds = 0.0F;

    private Image _Image;
    private int _ImageWidth;
    private int _ImageHeight;

    private Sprite _Sprite;
    private Color32[] _Pixels;
    private Texture2D _DefeatedTexture;

    private float _Time1 = 0;
    private float _Time2 = 0;

    #endregion

    #region Inspector Parameters

    [Tooltip("エフェクトを実行するかどうかを決める値です。")]
    public bool IsEnabled = false;

    [Tooltip("エフェクトの実行速度を決める値です。")]
    [Range(0.1F, 2.0F)]
    public float Speed = 1;

    [Tooltip("エフェクト開始直後に与える透過値です。")]
    [Range(0, 255)]
    public byte DefeatedWhiteStart = 0xCC;

    [Tooltip("ピクセルを崩す直前の透過値です。")]
    [Range(0, 255)]
    public byte DefeatedWhiteEnd = 0x20;

    [Tooltip("紫色に変更しない明度 (V) の最大値です。")]
    [Range(0, 255)]
    public byte LineBrightness = 0x32;

    [Tooltip("ピクセルを崩す斜線のピッチです。")]
    [Range(4, 10)]
    public int LinePitch = 6;

    #endregion

    #region Events

    public event EventHandler<Sprite> Completed;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _Image = GetComponent<Image>();
        _Sprite = _Image?.sprite;

        // 画像オブジェクトが正しく取得できない場合
        if (_Image == null || _Sprite == null || _Sprite.texture == null)
        {
            Debug.Log($"エフェクトの効果を与える {nameof(Image)} クラスのコンポーネントを取得できませんでした。");
            _Step = 100;
            return;
        }

        _CanEffect = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnabled || !_CanEffect) return;

        switch (_Step)
        {
            case 0:
                // 紫色に変化させる効果を与える
                ChangeDefeatedColor();
                _Step = 1;
                break;
            case 1:
                // ホワイトアウトするエフェクト
                _Time1 += Time.deltaTime;
                _Effect1Seconds = _WhiteoutSeconds / Speed;

                if (_Time1 >= _Effect1Seconds)
                {
                    _Time1 = _Effect1Seconds;
                    _Step = 2;
                }

                StartWhiteOut(_Time1, _Effect1Seconds, DefeatedWhiteStart, DefeatedWhiteEnd);
                break;
            case 2:
                // 崩れていくエフェクトのコルーチン予約
                _Effect2Seconds = _BreakSeconds / Speed;

                var timePitch = _Effect2Seconds / LinePitch;

                for (var i = 0; i < LinePitch; i++)
                {
                    StartCoroutine(this.DelayAction(timePitch * i, StartBreakDown, i, LinePitch));
                }
                _Step = 3;
                break;
            case 3:
                // ホワイトアウトするエフェクト
                _Time2 += Time.deltaTime;

                if (_Time2 >= _Effect2Seconds)
                {
                    _Time2 = _Effect2Seconds;
                    _Step = 4;
                }

                StartWhiteOut(_Time2,  _Effect2Seconds, DefeatedWhiteEnd, 0x00);
                break;
            case 4:
                // クリアを明示して終了する
                _Image.color = new Color32(0xFF, 0xFF, 0xFF, 0x00);
                _Step = 5;

                // 処理の終了を通知します
                Completed?.Invoke(this, _Sprite);
                break;
        }

    }

    #region Private Methods

    private void ChangeDefeatedColor()
    {
        _Pixels = _Sprite.texture.GetPixels32();

        _ImageWidth = (int)_Sprite.rect.width;
        _ImageHeight = (int)_Sprite.rect.height;

        _DefeatedTexture = new Texture2D(_ImageWidth, _ImageHeight, TextureFormat.RGBA32, false);
        _DefeatedTexture.filterMode = FilterMode.Point;

        for (int y = 0; y < _ImageHeight; y++)
        {
            for (int x = 0; x < _ImageWidth; x++)
            {
                var c = _Pixels[(_ImageWidth * y) + x];

                // ピクセルが透明のとき
                if (c.a == 0x00) continue;

                // 明度
                var v = GetBrightness(c);
                var vp = v / 255.0F; // 明度の割合

                if (v > LineBrightness) // 線と区別するライン
                {
                    // 紫色を基準に明度の割合を当てる
                    c.r = (byte)(0xEE * vp);
                    c.g = (byte)(0x66 * vp);
                    c.b = (byte)(0xFF * vp);
                }

                // すこし薄くする
                c.a = DefeatedWhiteStart;

                _Pixels[(_ImageWidth * y) + x] = c;
            }
        }

        _DefeatedTexture.SetPixels32(_Pixels);
        _DefeatedTexture.Apply();

        _Image.sprite = Sprite.Create(_DefeatedTexture, _Sprite.rect, _Sprite.pivot, _Sprite.pixelsPerUnit);
    }

    private void StartWhiteOut(float nowSeconds, float totalSeconds, byte whiteStart, byte whiteEnd)
    {
        var white = nowSeconds  * - ((whiteEnd - whiteStart) / totalSeconds);

        if (white >= 0.0F && white < 255.5F)
        {
            _Image.color = new Color32(0xFF, 0xFF, 0xFF, (byte)(whiteStart - Convert.ToByte(white)));
        }
    }

    private void StartBreakDown(int step, int linePitch)
    {
        for (int y = 0; y < _ImageHeight; y += linePitch)
        {
            for (int x = 0; x < _ImageWidth; x++)
            {
                var p = x % linePitch + step;

                if ((p + y) > linePitch)
                {
                    p = p - linePitch;
                }

                var pos = x + (p * _ImageWidth) + (y * _ImageWidth);

                if (pos < _Pixels.Length)
                {
                    var c = _Pixels[pos];

                    c.a = 0x00;
                    _Pixels[pos] = c;
                }
            }
        }

        _DefeatedTexture.SetPixels32(_Pixels);
        _DefeatedTexture.Apply();
    }

    private byte GetBrightness(Color32 color)
    {
        if (color.r >= color.g && color.r >= color.b) return color.r;
        if (color.g >= color.r && color.g >= color.b) return color.g;
        return color.b;
    }

    #endregion
}
