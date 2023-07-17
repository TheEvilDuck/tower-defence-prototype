using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleSpriteAnimatorComponent : MonoBehaviour
{
    [SerializeField] List<Sprite> _sprites;
    [SerializeField]float _framesPerSecond = 1f;
    [SerializeField]bool _loop;
    [SerializeField]bool _playOnAwake = false;
    [SerializeField]bool _turnOffAfterPlaying = false;

    private float _timer = 0;
    private bool _playing = false;
    private int _currentSpriteId = 0;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_playOnAwake)
        {
            Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playing)
        {
            if (_timer>=1f/_framesPerSecond)
            {
                _timer = 0;
                _currentSpriteId++;
                if (_currentSpriteId>=_sprites.Count)
                {
                    if (_loop)
                    {
                        _currentSpriteId = 0;
                    }
                    else
                    {
                        ForceStop();
                        return;
                    }
                }
                _spriteRenderer.sprite = _sprites[_currentSpriteId];
            }
            else
                _timer+=Time.deltaTime;

        }
    }

    public void Play()
    {
        _currentSpriteId = 0;
        _playing = true;
    }
    public void ForceStop()
    {
        _playing = false;
        if (_turnOffAfterPlaying)
            _spriteRenderer.sprite = null;
    }
}
