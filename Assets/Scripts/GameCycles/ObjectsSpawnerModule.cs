using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ObjectsSpawnerModule : ABaseModule
{
    private Obstacle _obstacle;
    private Dictionary<EObstacleType, List<ObstacleConfig>> _configs;
    private List<Obstacle> _generated;
    private IPoolingService _poolingService;
    public ObjectsSpawnerModule(IRuntimeUserData runtimeUserData,
        IPoolingService poolingService,
        Obstacle obstacle,
        Dictionary<EObstacleType, List<ObstacleConfig>> configs) : base(runtimeUserData)
    {
        _poolingService = poolingService;
        _obstacle = obstacle;
        _configs = configs;
    }

    public override void OnStart(IMainGameService gameCycleService)
    {
        _poolingService.Clear(_obstacle);
        base.OnStart(gameCycleService);
        _generated = new List<Obstacle>();
        _poolingService.Register(_obstacle);
        _runtimeData.Grid.Subscribe(GenerateObjects).AddTo(_compositeDisposable);
    }

    private void GenerateObjects(bool[] column)
    {
        if (column == null) return;
        bool isNextSpawned = false;
        

        for (int i = 0; i <column.Length; i++)
        {
            CustomBoxCollider2DRect customSize = new CustomBoxCollider2DRect()
            {
                Offset = Vector2.zero,
                Size = new Vector2(100, 100)
            };
            if (isNextSpawned)
            {
                isNextSpawned= false;
                continue;
            }
            EObstacleType type = EObstacleType.None;
            if (column[i])
            {
                if(i+1 < column.Length && column[i+1])
                {
                    type = EObstacleType.FatalObstacle;
                    isNextSpawned = true;
                    Vector2 thisPose = _runtimeData.RoadPositions[i];
                    Vector2 nextPose = _runtimeData.RoadPositions[i+1];
                    Vector2 size = new Vector2(150, Mathf.Abs(thisPose.y - nextPose.y) + 150);

                    customSize = new CustomBoxCollider2DRect()
                    {

                        Size = size,
                        Offset = new Vector2(0, size.y/2f - 50)

                    };
                }
                else
                {
                    type = EObstacleType.DefaultObstacle;
                }
            }
            else
            {
                if(UnityEngine.Random.Range(0, 100) > 80)
                    type = EObstacleType.Eat;
            }
            if(type != EObstacleType.None)
            {
                var newObj = _poolingService.GetObject(_obstacle);
                newObj.transform.SetParent(_runtimeData.RoadRects[i]);
                newObj.Configure(GetRandomConfig(type), customSize);
                newObj.gameObject.SetActive(true);
                _generated.Add(newObj);
                newObj.Rect.anchoredPosition = Vector2.right * _gameCycleService.GamePlayScene.GameField.rect.width * 2;
            }
        }
    }

    private ObstacleConfig GetRandomConfig(EObstacleType type)
    {
        return _configs[type][UnityEngine.Random.Range(0, _configs[type].Count)];
    }

    public override void OnUpdate()
    {
        for (int i = 0; i < _generated.Count; i++)
        {
            if (_generated[i].transform.position.x < -150)
            {
                _poolingService.ReleaseObject(_generated[i], _obstacle.name);
                _generated.Remove(_generated[i]);
                continue;
            }

            _generated[i].Rect.Translate(Vector3.left * _runtimeData.CurrentSpeed * Time.deltaTime);
        }
    }

    public override void OnEnd()
    {
        base.OnEnd();
        if(_generated != null)
        for (int i = 0; i < _generated.Count; i++)
        {
            _poolingService.ReleaseObject(_generated[i], _obstacle.name);
            _generated.Remove(_generated[i]);
        }
    }
}
