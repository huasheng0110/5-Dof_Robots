# 项目说明
本项目旨在Unity平台上，借助ML-agents强化学习算法实现5自由度机械臂的路径规划问题。场景搭建大致为：5自由度机械臂模型，小球(作为Goal)。模型最终的结果应该为：机械臂能够很好地抓取小球，即场景中小球变色的频率越来越高(小球变色用来指示机械臂已接触到目标)

[学习链接B站转载视频地址【使用 Unity ML Agent 训练机器人手臂 _ Part1/5】]( https://www.bilibili.com/video/BV1v14y1y7a2/?share_source=copy_web&vd_source=2a87d5b54021591bb121552e3efc3996)
[YouTube视频地址](https://youtu.be/HOUPkBF-yv0?si=1YPg3ZrAycaL-KRr)

- 项目树
  - 设置具有可配置关节的模型
  - 设置目标行为
  - 为机械臂编写 MLAgent 脚本
  - 训练机器人
  - 使用训练好的神经网络


# 项目环境
[环境配置参考](https://unity-technologies.github.io/ml-agents/Installation/)
Unity 6000.0.23f1
Python 3.10.12
ML-agents release_22
pip3 install torch~=2.2.1 --index-url https://download.pytorch.org/whl/cu121
