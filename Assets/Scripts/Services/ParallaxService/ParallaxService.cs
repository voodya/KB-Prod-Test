using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IParallaxService
{
    void PlayParallax();
    void RegisterLayer(RectTransform transform, int layer);
    void RegisterLayer(List<RectTransform> transform, int layer);
    void ReleaseLayer(RectTransform transform, int layer);
    void ReleaseLayer(List<RectTransform> transform, int layer);
}

public class ParallaxService : IParallaxService
{
    private List<List<RectTransform>> _layers = new(3) 
    {
        new List<RectTransform>(5),
        new List<RectTransform>(5),
        new List<RectTransform>(5),
        new List<RectTransform>(5),
    };
    private List<float> _speeds;

    public ParallaxService(List<float> speeds)
    {
        _speeds = speeds;
    }

    public void RegisterLayer(RectTransform transform, int layer)
    {
        _layers[layer].Add(transform);
    }

    public void RegisterLayer(List<RectTransform> transform, int layer)
    {
        _layers[layer].AddRange(transform);
    }

    public void PlayParallax()
    {
        Observable.EveryUpdate().Subscribe(UpdateParallax);
    }

    public void ReleaseLayer(RectTransform transform, int layer)
    {
        _layers[layer].Remove(transform);
    }

    public void ReleaseLayer(List<RectTransform> transform, int layer)
    {
        for (int i = 0; i < transform.Count; i++)
        {
            _layers[layer].Remove(transform[i]);
        }
    }


    private void UpdateParallax(long obj)
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            for (int j = 0; j < _layers[i].Count; j++)
            {
                if (_layers[i][j] == null) return;
                _layers[i][j].anchoredPosition += Vector2.left * _speeds[i] * Time.deltaTime;
                if (_layers[i][j].anchoredPosition.x < -_layers[i][j].rect.width)
                {
                    Vector2 newPos = _layers[i][j].anchoredPosition;
                    newPos.x += 2 * _layers[i][j].rect.width;
                    _layers[i][j].anchoredPosition = newPos;
                }
            }
        }
    }


}
