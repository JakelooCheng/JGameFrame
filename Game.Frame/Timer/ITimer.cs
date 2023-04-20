namespace Game.Frame.Timer
{
    public interface ITimer
    {
        /// <summary>
        /// �Ƿ���Ч
        /// </summary>
        bool IsInvalid { get; }

        /// <summary>
        /// ��ʱ��id
        /// </summary>
        long Id { get; }

        /// <summary>
        /// �Ƿ���ͣ
        /// </summary>
        bool Pause { get; set; }

        /// <summary>
        /// ѭ���������� С�ڵ��� 0 ʱ����ʾ����ѭ��
        /// </summary>
        long LoopTime { get; }

        /// <summary>
        /// ��ѭ������
        /// </summary>
        long LoopedTime { get; }

        /// <summary>
        /// ѭ�����
        /// </summary>
        int IntervalMS { get; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        float RunDurationMS { get; }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        float StartMS { get; }

        /// <summary>
        /// �Ƿ��Ǳ�ʱ������Ӱ��ļ�ʱ��
        /// </summary>
        bool IsScaled { get; }
    }
}