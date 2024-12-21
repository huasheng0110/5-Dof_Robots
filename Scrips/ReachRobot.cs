using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ReacheRobot : Agent
{// 五个自由度的机械臂，有五个关节
    public GameObject armA;
    public GameObject armB;
    public GameObject armC;
    public GameObject armD;
    public GameObject armE;
    Rigidbody armARb;
    Rigidbody armBRb;
    Rigidbody armCRb;
    Rigidbody armDRb;
    Rigidbody armERb;
    // 声明机械臂的一个末端和一个目标
    public GameObject hand;
    public GameObject goal;
    public GameObject goalon;
    public GameObject Obstacle;

    public float m_GoalHeight = 5f;
    float m_GoalRadius; //Radius of the goal zone
    float m_GoalDegree; //How much the goal rotates
    float m_GoalSpeed; //Speed of the goal rotation
    float m_GoalDeviation; //How much goes up and down
    float m_GoalDeviationFreg; //Frequency of the goal up and down movement


    public override void Initialize()
    {
        // 初始化代码
        armARb = armA.GetComponent<Rigidbody>();
        armBRb = armB.GetComponent<Rigidbody>();
        armCRb = armC.GetComponent<Rigidbody>();
        armDRb = armD.GetComponent<Rigidbody>();
        armERb = armE.GetComponent<Rigidbody>();


        // // 打印关节的初始状态
        // PrintInitialState("Arm A", armA);
        // PrintInitialState("Arm B", armB);
        // PrintInitialState("Arm C", armC);
        // PrintInitialState("Arm D", armD);
        // PrintInitialState("Arm E", armE);
        // SetResetParameters();
    }


    public override void OnEpisodeBegin()
    {
        // 每个训练周期重置机械臂位置代码
        armA.transform.position = new Vector3(0.00f, 0.53f, 0.0f) + transform.position;
        armA.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
        armARb.linearVelocity = Vector3.zero;
        armARb.angularVelocity = Vector3.zero;

        armB.transform.position = new Vector3(-0.1f, 1.31f, 0.01f) + transform.position;
        armB.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
        armBRb.linearVelocity = Vector3.zero;
        armBRb.angularVelocity = Vector3.zero;

        armC.transform.position = new Vector3(-0.09f, 2.32f, -0.01f) + transform.position;
        armC.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
        armCRb.linearVelocity = Vector3.zero;
        armCRb.angularVelocity = Vector3.zero;

        armD.transform.position = new Vector3(-0.23f, 3.07f, -0.03f) + transform.position;
        armD.transform.rotation = Quaternion.Euler(270f, 0f, 0f);
        armDRb.linearVelocity = Vector3.zero;
        armDRb.angularVelocity = Vector3.zero;

        armE.transform.position = new Vector3(0.36f, 4.31f, -0.13f) + transform.position;
        armE.transform.rotation = Quaternion.Euler(0f, 270f, 270f);
        armERb.linearVelocity = Vector3.zero;
        armERb.angularVelocity = Vector3.zero;


        SetResetParameters();
        m_GoalDegree += m_GoalSpeed;
        UpdateGoalPosition();
        UpdateGoalonPosition();
    }

    public void SetResetParameters()
    {
        m_GoalRadius = Random.Range(1f, 2f);
        m_GoalDegree = Random.Range(0f, 360f);
        m_GoalSpeed = Random.Range(-2f, 2f);
        m_GoalDeviation = Random.Range(-1f, 1f);
        m_GoalDeviationFreg = Random.Range(0f, 3.14f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 收集观察数据
        sensor.AddObservation(armA.transform.localPosition);
        sensor.AddObservation(armA.transform.rotation);
        sensor.AddObservation(armARb.linearVelocity);
        sensor.AddObservation(armARb.angularVelocity);

        sensor.AddObservation(armB.transform.localPosition);
        sensor.AddObservation(armB.transform.rotation);
        sensor.AddObservation(armBRb.linearVelocity);
        sensor.AddObservation(armBRb.angularVelocity);

        sensor.AddObservation(armC.transform.localPosition);
        sensor.AddObservation(armC.transform.rotation);
        sensor.AddObservation(armCRb.linearVelocity);
        sensor.AddObservation(armCRb.angularVelocity);

        sensor.AddObservation(armD.transform.localPosition);
        sensor.AddObservation(armD.transform.rotation);
        sensor.AddObservation(armDRb.linearVelocity);
        sensor.AddObservation(armDRb.angularVelocity);

        sensor.AddObservation(armE.transform.localPosition);
        sensor.AddObservation(armE.transform.rotation);
        sensor.AddObservation(armERb.linearVelocity);
        sensor.AddObservation(armERb.angularVelocity);



        sensor.AddObservation(goal.transform.localPosition);
        sensor.AddObservation(hand.transform.localPosition);

        sensor.AddObservation(m_GoalSpeed);

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // 执行动作代码
        var continuousActions = actionBuffers.ContinuousActions;
        armARb.AddTorque(new Vector3(0f, continuousActions[0], 0f));
        armBRb.AddTorque(new Vector3(0f, continuousActions[1], 0f));
        armCRb.AddTorque(new Vector3(0f, continuousActions[2], 0f));
        armDRb.AddTorque(new Vector3(0f, continuousActions[3], 0f));
        armERb.AddTorque(new Vector3(0f, 0f, 0f));


        m_GoalDegree += m_GoalSpeed;
        UpdateGoalPosition();

        AddReward(-0.001f);

        // 计算hand与goal之间的距离
        float distanceToGoal = Vector3.Distance(hand.transform.position, goal.transform.position);
        // 根据距离增加奖励，距离越近奖励越大
        float reward = 1.0f / (distanceToGoal + 1);//防止分母为0
        AddReward(reward);


        if (distanceToGoal < 0.1f)
        {
            SetReward(2f);
            EndEpisode();
        }

        // 如果hand掉落，结束训练周期
        if (distanceToGoal > 4.0f)
        {
            EndEpisode();
        }

      
    }

    // 机械臂碰撞障碍物，结束训练周期
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Obstacle)
        {
            EndEpisode();
        }
    }
    // 目标goal与obstacle碰撞，结束训练周期
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Obstacle)
        {
            EndEpisode();
        }
    }

    void UpdateGoalPosition()
    {
        var m_GoalDegree_rad = m_GoalDegree * Mathf.PI / 180f;
        var goalx = m_GoalRadius * Mathf.Cos(m_GoalDegree_rad);
        var goalz = m_GoalRadius * Mathf.Sin(m_GoalDegree_rad);
        var goaly = m_GoalHeight + m_GoalDeviation * Mathf.Cos(m_GoalDeviationFreg * m_GoalDegree_rad);
        goal.transform.position = new Vector3(goalx, goaly, goalz) + transform.position;
    }

    // update the goalon position to the goal position
     void UpdateGoalonPosition()
    {
        goalon.transform.position = goal.transform.position;
    }


    // 打印初始状态的辅助函数
    private void PrintInitialState(string armName, GameObject arm)
    {
        Vector3 position = arm.transform.position;
        Quaternion rotation = arm.transform.rotation;

        Debug.Log($"{armName} Initial Position: {position}");
        Debug.Log($"{armName} Initial Rotation: {rotation.eulerAngles}");

    }
        //public override void Heuristic(in ActionBuffers actionsOut)
        //{
        //    // 手动控制，用于测试
        //    var continuousActionsOut = actionsOut.ContinuousActions;
        //    continuousActionsOut[0] = Input.GetAxis("Horizontal");
        //    continuousActionsOut[1] = Input.GetAxis("Vertical");
        //    continuousActionsOut[2] = Input.GetAxis("Depth");
        //}
    }