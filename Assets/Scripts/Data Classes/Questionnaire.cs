using System.Collections.Generic;
using System;

[Serializable]
public class QuestionnaireData
{
    public int gender;
    public int ageGroup;
    public int comfort1;
    public int comfort2;
    public int comfort3;
    public int trigger1;
    public int trigger2;
    public List<int> trigger3; // Multi-answer as a list

    public QuestionnaireData(int gender, int ageGroup, int comfort1, int comfort2,
                             int comfort3, int trigger1, int trigger2,
                             List<int> trigger3)
    {
        this.gender = gender;
        this.ageGroup = ageGroup;
        this.comfort1 = comfort1;
        this.comfort2 = comfort2;
        this.comfort3 = comfort3;
        this.trigger1 = trigger1;
        this.trigger2 = trigger2;
        this.trigger3 = trigger3;
    }
}
