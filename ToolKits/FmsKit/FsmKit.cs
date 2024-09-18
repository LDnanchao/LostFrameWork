﻿namespace LostFramework
{
    /*
     * 状态机，由于常规的有限状态机，每个状态都需要单独去维护，参考MMTool的有限状态奇
     * 将有限状态机拆分为 大脑，行为，判断，大脑负重执行状态机，行为则动作，判断负重状态切换，
     * 使用这种方式，将代码和模块进行分离，对于更复杂的行为，需要使用行为树进行处理，这里只是提供一个简易状态机，以用于功能快速开发
     */
    public class FsmKit
    {
        
    }
}