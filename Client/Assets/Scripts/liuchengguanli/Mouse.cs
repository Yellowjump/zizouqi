using liuchengguanli;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Mouse : MonoBehaviour
{
    private bool GetOrNotGetQizi = false;
    GameObject qiziobj;
    EntityQizi qizi;
    EntityQizi qiziother;
    Vector3 qiziObj_oldlocation;
    float timenow = 0;
    void Update()
    {
        if (!GetOrNotGetQizi)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "qizi")
                {
                    for (int i=0;i<QiziGuanLi.Instance.QiziList.Count;i++)
                    {
                        if (hit.transform.localPosition == QiziGuanLi.Instance.QiziList[i].GObj.transform.localPosition)
                        {
                            qizi = QiziGuanLi.Instance.QiziList[i];
                        }
                    }
                    //Log.Info("hfk:" + hit.transform.tag);
                    qiziobj = hit.collider.gameObject;
                    qiziObj_oldlocation = qiziobj.transform.position;
                    GetOrNotGetQizi = true;
                }
            }
        }
        else //if (timenow <= Time.time)
        {
            timenow = Time.time + 0.1f;
            int findotherQizi = -1;
            int findzijiqizi = -1;
            int findqige = -1;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i=0;i<hits.Length;i++)//�ƶ������ק����
            {
                if (hits[i].transform.tag == "qige")
                {
                    qiziobj.transform.localPosition = hits[i].point + new Vector3(0, 0.2f, 0);
                    findqige = i;
                }
                else if(hits[i].transform.name == "qipan")
                {
                    qiziobj.transform.localPosition = hits[i].point + new Vector3(0, 0.2f, 0);
                }
                if (hits[i].transform.tag == "qizi" && hits[i].transform!=qiziobj.transform)
                {
                    findotherQizi = i;
                }
                if (hits[i].transform == qiziobj.transform)
                {
                    findzijiqizi = i;
                }
            }
            if (Input.GetMouseButtonDown(0))//�ж����߼�⣬Ӧ�÷������ӵ�������ϻ��Ǻ��Ѿ����õ����ӽ���λ�ã������޷��������ӣ��ص�ԭλ��
            {
                //Log.Info("hfk:findqige:"+findqige+"findotherqizi"+findotherQizi);
                if (findqige != -1)//��겻������ϣ��Ż�ԭλ
                {
                    if (findotherQizi == -1)//�ո���
                    {
                        //�����ק�������ǳ��µ�
                        if (qiziObj_oldlocation.z == -4.5)
                        {   //�ŵ�λ���ǳ���
                            if (hits[findqige].transform.localPosition.z != -4.5)
                            {   //���ҳ���������С�ڵ��ڵȼ�����Ҫ��changxia[]����λ����Ϊ-1
                                if (QiziGuanLi.Instance.QiziCSList.Count < Dengji.Instance.getDj())
                                {
                                    qiziobj.transform.localPosition = hits[findqige].transform.localPosition;
                                    QiziGuanLi.Instance.changxia[(int)qiziObj_oldlocation.x + 4] = -1;
                                    QiziGuanLi.Instance.QiziCSList.Add(qizi);
                                }
                                else
                                {
                                    qiziobj.transform.localPosition = qiziObj_oldlocation;
                                }
                            }
                            else //�ŵ�λ���ǳ���
                            {
                                qiziobj.transform.localPosition = hits[findqige].transform.localPosition;
                                QiziGuanLi.Instance.changxia[(int)qiziObj_oldlocation.x + 4] = -1;
                                QiziGuanLi.Instance.changxia[(int)hits[findqige].transform.position.x + 4] = qizi.Index;
                            }
                        }
                        else//��������ǳ��ϵ�
                        {   //����ŵ�λ���ǳ���
                            if (hits[findqige].transform.localPosition.z==-4.5)
                            {
                                QiziGuanLi.Instance.changxia[(int)hits[findqige].transform.localPosition.x + 4] = qizi.Index;
                                QiziGuanLi.Instance.QiziCSList.Remove(qizi);
                            }
                            qiziobj.transform.localPosition = hits[findqige].transform.localPosition;
                        }
                    }
                    else//������б������
                    {
                        for (int i = 0; i < QiziGuanLi.Instance.QiziList.Count; i++)
                        {
                            if (hits[findotherQizi].transform.localPosition == QiziGuanLi.Instance.QiziList[i].GObj.transform.localPosition)
                            {
                                qiziother = QiziGuanLi.Instance.QiziList[i];
                            }
                        }
                        //�����ק�������ǳ��µ�
                        if (qiziObj_oldlocation.z == -4.5)
                        {   //�ŵ�����λ���ǳ���
                            if (hits[findqige].transform.localPosition.z != -4.5)
                            {   
                                qiziobj.transform.localPosition = hits[findqige].transform.localPosition;
                                qiziother.GObj.transform.localPosition = qiziObj_oldlocation;
                                QiziGuanLi.Instance.changxia[(int)qiziObj_oldlocation.x + 4] = qiziother.Index;
                                QiziGuanLi.Instance.QiziCSList.Remove(qiziother);
                                QiziGuanLi.Instance.QiziCSList.Add(qizi);
                            }
                            else //�ŵ�λ���ǳ���
                            {
                                QiziGuanLi.Instance.changxia[(int)qiziObj_oldlocation.x + 4] = qiziother.Index;
                                QiziGuanLi.Instance.changxia[(int)qiziother.GObj.transform.localPosition.x + 4] = qizi.Index;
                                qiziobj.transform.localPosition = hits[findqige].transform.localPosition;
                                qiziother.GObj.transform.localPosition = qiziObj_oldlocation;
                            }
                        }
                        else//��������ǳ��ϵ�
                        {   //����ŵ�λ���ǳ���
                            if (hits[findqige].transform.localPosition.z == -4.5)
                            {
                                QiziGuanLi.Instance.changxia[(int)hits[findqige].transform.localPosition.x + 4] = qizi.Index;
                                QiziGuanLi.Instance.QiziCSList.Remove(qizi);
                                QiziGuanLi.Instance.QiziCSList.Add(qiziother);
                            }//�ŵ�λ���ǳ��ϵ����ӣ��൱�ڽ���������λ��
                            qiziother.GObj.transform.position = qiziObj_oldlocation;
                            qiziobj.transform.localPosition = hits[findqige].transform.localPosition;
                        }
                    }
                }
                else 
                {
                    qiziobj.transform.localPosition = qiziObj_oldlocation;
                }
                qiziobj = null;
                GetOrNotGetQizi = false;
            }
        }
        //else if(Input.GetMouseButtonDown(0))
        //{
        //    timenow = Time.time + 0.1f;
        //    int findotherQizi = -1;
        //    int findzijiqizi = -1;
        //    int findqige = -1;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit[] hits = Physics.RaycastAll(ray);
        //    for (int i = 0; i < hits.Length; i++)//�ƶ������ק����
        //    {
        //        if (hits[i].transform.tag == "qige")
        //        {
        //            qiziobj.transform.localPosition = hits[i].point + new Vector3(0, 0.2f, 0);
        //            findqige = i;
        //        }
        //        else if (hits[i].transform.name == "qipan")
        //        {
        //            qiziobj.transform.localPosition = hits[i].point + new Vector3(0, 0.2f, 0);
        //        }
        //        if (hits[i].transform.tag == "qizi" && hits[i].transform != qiziobj.transform)
        //        {
        //            findotherQizi = i;
        //        }
        //        if (hits[i].transform == qiziobj.transform)
        //        {
        //            findzijiqizi = i;
        //        }
        //    }
        //    if (Input.GetMouseButtonDown(0))//�ж����߼�⣬Ӧ�÷������ӵ�������ϻ��Ǻ��Ѿ����õ����ӽ���λ�ã������޷��������ӣ��ص�ԭλ��
        //    {
        //        if (findqige == -1)//��겻������ϣ��Ż�ԭλ
        //        {
        //            qiziobj.transform.localPosition = qiziObj_oldlocation;
        //        }
        //        else
        //        {
        //            if (findotherQizi == -1)//�ո���
        //            {
        //                qiziobj.transform.localPosition = hits[findqige].transform.localPosition;
        //            }
        //            else//������б������
        //            {
        //                qiziobj.transform.localPosition = hits[findqige].transform.localPosition;
        //                hits[findotherQizi].transform.localPosition = qiziObj_oldlocation;
        //            }
        //        }
        //    }
        //    qiziobj = null;
        //    GetOrNotGetQizi = false;
        //}
    }
}
