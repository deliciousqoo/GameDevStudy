using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudentOwnedStates
{
    public class RestAndSleep : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrentLocation = Locations.SweetHome;
            entity.Stress = 0;

            entity.PrintText("집에 들어온다. 행복한 우리집~ 집에 오니 스트레스가 사라졌다.");
            entity.PrintText("침대에 누워 잠을 잔다");
        }

        public override void Execute(Student entity)
        {
            entity.PrintText("zzZ~ zz~ zzzzZ~~");

            if (entity.Fatigue > 0)
            {
                entity.Fatigue -= 10;
            }
            else
            {
                entity.ChangeState(StudentStates.StudyHard);
            }
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("침대를 정리하고 집 밖으로 나간다.");
        }
    }
    public class StudyHard : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrentLocation = Locations.Library;
            entity.PrintText("공부를 하기 위해 도서관에 왔다.. 힘차..게.. 공부를 하자");
        }

        public override void Execute(Student entity)
        {
            entity.PrintText("공부 공부 공부 공부...");

            entity.Knowledge++;
            entity.Stress++;
            entity.Fatigue++;

            if (entity.Knowledge >= 3 && entity.Knowledge <= 10)
            {
                int isExit = Random.Range(0, 2);
                if (isExit == 1 || entity.Knowledge == 10)
                {
                    entity.ChangeState(StudentStates.TakeAExam);
                }
            }

            if (entity.Stress >= 20)
            {
                entity.ChangeState(StudentStates.PlayAGame);
            }
            if (entity.Fatigue >= 50)
            {
                entity.ChangeState(StudentStates.RestAndSleep);
            }
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("자리를 정리하고 도서관을 나간다.");
        }
    }
    public class TakeAExam : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrentLocation = Locations.LectureRoom;
            entity.PrintText("강의실에 들어간다. 시험지를 받았다.");
        }
        public override void Execute(Student entity)
        {
            int examScore = 0;

            if(entity.Knowledge == 10)
            {
                examScore = 10;
            }
            else
            {
                int randIndex = Random.Range(0, 10);
                examScore = randIndex < entity.Knowledge ? Random.Range(6, 11) : Random.Range(1, 6);
            }

            entity.Knowledge = 0;
            entity.Fatigue += Random.Range(5, 11);

            entity.TotalScore += examScore;
            entity.PrintText($"시험 성적({examScore}), 총점({entity.TotalScore})");
        
            if(entity.TotalScore >= 100)
            {
                GameController.Stop(entity);
                return;
            }

            if(examScore <= 3)
            {
                entity.ChangeState(StudentStates.HitTheBottle);
            }
            else if(examScore <= 7)
            {
                entity.ChangeState(StudentStates.StudyHard);
            }
            else
            {
                entity.ChangeState(StudentStates.PlayAGame);
            }
        }
        public override void Exit(Student entity)
        {
            entity.PrintText("강의실 문을 열고 밖을 나온다.");
        }
    }
    public class PlayAGame : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrentLocation = Locations.PCRoom;

            entity.PrintText("한시간만.. 딱 한시간만 놀아야지.. PC방으로 들어간다.");
        }

        public override void Execute(Student entity)
        {
            entity.PrintText("건전하게?? 게임을 즐긴다..");

            int randState = Random.Range(0, 10);
            if (randState == 0 || randState == 9)   
            {
                entity.Stress += 20;
                entity.ChangeState(StudentStates.HitTheBottle);
            }
            else
            {
                entity.Stress--;
                entity.Fatigue += 2;

                if(entity.Stress <= 0)
                {
                    entity.ChangeState(StudentStates.StudyHard);
                }
            }
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("PC방에서 나온다.");
        }
    }
    public class HitTheBottle : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrentLocation = Locations.Pub;
            entity.PrintText("술이나 한잔할까? 술집으로 들어간다.");
        }

        public override void Execute(Student entity)
        {
            entity.PrintText("술을 마신다.");

            entity.Stress -= 5;
            entity.Fatigue += 5;

            if (entity.Stress <= 0 || entity.Fatigue >= 50)
            {
                entity.ChangeState(StudentStates.RestAndSleep);
            }
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("그만 마셔야지.. 술집에서 나온다");
        }
    }
}
