using System;

/// <summary>
/// StringSimilarity is used to compute the similarity between two strings
/// Algorithms shamelessly taken from : https://social.technet.microsoft.com/wiki/contents/articles/26805.c-calculating-percentage-similarity-of-2-strings.aspx
/// Original Author : Adnan Umer, (Revised by RajeeshMenoth)
///</summary>
public static class StringSimilarity {


    public static int LevenshteinDistance(string source, string target){
        if ((source == null) || (target == null)) return 0;
        if (source.Length == 0 || target.Length == 0) return 0;
        if (source == target) return source.Length;

        int sourceWordLen = source.Length;
        int targetWordLen = target.Length;

        if (sourceWordLen == 0) return targetWordLen;
        if (targetWordLen == 0) return sourceWordLen;
        
        int[,] distance = new int[sourceWordLen+1, targetWordLen+1];
        for(int i = 0; i <= sourceWordLen; distance[i, 0] = i++);
        for(int j = 0; j <= targetWordLen; distance[0, j] = j++);

        for(int i = 1; i <= sourceWordLen; i++){
            for(int j = 1; j <= targetWordLen; j++){
                int cost = (target[j-1]) == (source[i-1]) ? 0 : 1;
                distance[i, j] = Math.Min(Math.Min(distance[i-1, j]+1, distance[i, j-1]+1), distance[i-1, j-1]+cost);
            }
        }
        return distance[sourceWordLen, targetWordLen];
    }

    public static int DamerauLevenshteinDistance(string string1, string string2){
        /// http://mihkeltt.blogspot.com/2009/04/dameraulevenshtein-distance.html
        if(String.IsNullOrEmpty(string1)){
            if(String.IsNullOrEmpty(string2)) return string2.Length;
            return 0;
        }
        if(String.IsNullOrEmpty(string2)){
            if(String.IsNullOrEmpty(string1)) return string1.Length;
            return 0;
        }

        int length1 = string1.Length;
        int length2 = string1.Length;

        int[,] d = new int[length1 + 1, length2 + 1];

        int cost, del, ins, sub;
        for(int i = 0 ; i <= d.GetUpperBound(0); i++) d[i, 0] = i;
        for(int i = 0 ; i <= d.GetUpperBound(1); i++) d[0, i] = i;

        for(int i = 1; i <= d.GetUpperBound(0); i++){
            for(int j = 1; j <= d.GetUpperBound(1); j++){
                if(string1[i-1] == string2[j-1]) cost = 0;
                else cost = 1;
                del = d[i-1, j] + 1;
                ins = d[i, j-1] + 1;
                sub = d[i-1, j-1] + cost;

                d[i, j] = Math.Min(del, Math.Min(ins, sub));
                if(i > 1 && j > 1 && string1[i-1] == string2[j-2] && string1[i-2]==string2[j-1]){
                    d[i, j] = Math.Min(d[i, j], d[i-2, j-2]+cost);
                }
            }
        }        
        return d[d.GetUpperBound(0),d.GetUpperBound(1)];
    }

    public static float PercentageSimilar(string source, string target){
        if ((source == null) || (target == null)) return 0.0f;
        if (source.Length == 0 || target.Length == 0) return 0.0f;
        if (source == target) return 1.0f;
        float stepsDifference = (float)DamerauLevenshteinDistance(source, target);
        float longest = (float)Math.Min(source.Length, target.Length);
        return 1.0f - (stepsDifference/longest);
    }
}