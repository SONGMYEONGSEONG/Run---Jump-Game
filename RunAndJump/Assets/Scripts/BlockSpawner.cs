using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum defineNum
{
    Blokc_End_posX = -17, //�ش� x��ǥ�� ������ pivot�� ������ ������� ��ǥ 
}

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] List<Block> block_list;
    [SerializeField] float blockSummonCoolTime = 2.0f;

    [SerializeField] float startPosX = 17.0f; //���� ���۵Ǵ� ��ġ x��ǥ
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

            //���� ������ 10% ��ŭ �� �����ֱ� ���� -> ���� �ʰ� ������
            LevelSuummonCoolTime = blockSummonCoolTime + ( (GameManager.Instance.CurLevel - 1) *  0.1f);
            
            yield return  new WaitForSeconds(LevelSuummonCoolTime);
        }
    }

   private void MakeBlock()
    {
        //��� ����Ȯ���� 100% ���� 75%���� ������
        //����Ȯ���� 6���� ���ϱ����� ���� �� ���ķδ� 50% ����
        int curStageLv = GameManager.Instance.CurLevel;
        int SuumonPercent = curStageLv >= 5 ? 5 : (curStageLv - 1);

        if (SuumonPercent <= Random.Range(0, 20))
        {

            int type = Random.Range(0, block_list.Count);

            Block block = Instantiate(block_list[type]);
            block.gameObject.SetActive(false);

            float x = startPosX;
            //y ���� : -4.5 ,-1.5,1.5, 4.5
        
            //�ѹ� ����� ��ġ�� �ٽ� ������ �ʴ� �Լ�
            int y;
            do
            {
                y = Random.Range(0, blockSpawnPosY.Count);
            }
            while (isSpawnBlockIndex == y);
            isSpawnBlockIndex = y;


            block.Block_pos = new Vector3(x, blockSpawnPosY[y], 0);
            //���� ��ġ�� 10% ��ŭ ��� ���� speed ����
            if (curStageLv > 1) block.Speed += (curStageLv * 0.1f);
            block.gameObject.SetActive(true);
        }
    }
}
