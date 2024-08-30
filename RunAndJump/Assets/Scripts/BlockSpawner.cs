using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum defineNum
{
    Blokc_End_posX = -17, //해당 x좌표에 닿으면 pivot이 닿으면 사라지는 좌표 
}

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] List<Block> block_list;
    [SerializeField] float blockSummonCoolTime = 2.0f;

    [SerializeField] float startPosX = 17.0f; //생성 시작되는 위치 x좌표
    [SerializeField] float startPosY_Min = -5.0f;
    [SerializeField] float startPosY_Max = 2.0f;

    Coroutine coroutineBlockSpanw;
    [SerializeField] List<float> blockSpawnPosY;

    [SerializeField] bool isstop = false;

    int isSpawnBlockIndex = -1;

    public void BlockSpawnerON()
    {
        if (!isstop)
        {
            if (coroutineBlockSpanw == null)
            {
                coroutineBlockSpanw = StartCoroutine(BlockSpanw());
            }
            else
            {
                StopCoroutine(coroutineBlockSpanw);
                coroutineBlockSpanw = StartCoroutine(BlockSpanw());
            }
        }
    }

    public void BlockSpawnerOFF()
    {
        if (coroutineBlockSpanw != null)
        {
            StopCoroutine(coroutineBlockSpanw);
        }
    }

    IEnumerator BlockSpanw()
    {
        float LevelSuummonCoolTime;

        while (true)
        {
            MakeBlock();

            //현재 레벨의 10% 만큼 블럭 생성주기 증가 -> 블럭이 늦게 등장함
            LevelSuummonCoolTime = blockSummonCoolTime + ( (GameManager.Instance.CurLevel - 1) *  0.1f);
            
            yield return  new WaitForSeconds(LevelSuummonCoolTime);
        }
    }

   private void MakeBlock()
    {
        //블록 생성확률이 100% 에서 75%까지 떨어짐
        //생성확률은 6레벨 이하까지만 적용 그 이후로는 50% 동일
        int curStageLv = GameManager.Instance.CurLevel;
        int SuumonPercent = curStageLv >= 5 ? 5 : (curStageLv - 1);

        if (SuumonPercent <= Random.Range(0, 20))
        {

            int type = Random.Range(0, block_list.Count);

            Block block = Instantiate(block_list[type]);
            block.gameObject.SetActive(false);

            float x = startPosX;
            //y 범위 : -4.5 ,-1.5,1.5, 4.5
        
            //한번 생겼던 위치에 다시 생기지 않는 함수
            int y;
            do
            {
                y = Random.Range(0, blockSpawnPosY.Count);
            }
            while (isSpawnBlockIndex == y);
            isSpawnBlockIndex = y;


            block.Block_pos = new Vector3(x, blockSpawnPosY[y], 0);
            //레벨 수치의 10% 만큼 모든 블럭의 speed 증가
            if (curStageLv > 1) block.Speed += (curStageLv * 0.1f);
            block.gameObject.SetActive(true);
        }
    }
}
