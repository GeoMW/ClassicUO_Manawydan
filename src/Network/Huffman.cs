﻿#region license

// Copyright (c) 2021, andreakarasho
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by andreakarasho - https://github.com/andreakarasho
// 4. Neither the name of the copyright holder nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;

namespace ClassicUO.Network
{
    internal static class Huffman
    {
        #region Decompression Tree

        private static readonly int[,] dec_tree =
        {
            /*node*/ /*leaf0 leaf1*/
            /* 0*/
            {
                2, 1
            },
            /* 1*/
            {
                4, 3
            },
            /* 2*/
            {
                0, 5
            },
            /* 3*/
            {
                7, 6
            },
            /* 4*/
            {
                9, 8
            },
            /* 5*/
            {
                11, 10
            },
            /* 6*/
            {
                13, 12
            },
            /* 7*/
            {
                14, -256
            },
            /* 8*/
            {
                16, 15
            },
            /* 9*/
            {
                18, 17
            },
            /* 10*/
            {
                20, 19
            },
            /* 11*/
            {
                22, 21
            },
            /* 12*/
            {
                23, -1
            },
            /* 13*/
            {
                25, 24
            },
            /* 14*/
            {
                27, 26
            },
            /* 15*/
            {
                29, 28
            },
            /* 16*/
            {
                31, 30
            },
            /* 17*/
            {
                33, 32
            },
            /* 18*/
            {
                35, 34
            },
            /* 19*/
            {
                37, 36
            },
            /* 20*/
            {
                39, 38
            },
            /* 21*/
            {
                -64, 40
            },
            /* 22*/
            {
                42, 41
            },
            /* 23*/
            {
                44, 43
            },
            /* 24*/
            {
                45, -6
            },
            /* 25*/
            {
                47, 46
            },
            /* 26*/
            {
                49, 48
            },
            /* 27*/
            {
                51, 50
            },
            /* 28*/
            {
                52, -119
            },
            /* 29*/
            {
                53, -32
            },
            /* 30*/
            {
                -14, 54
            },
            /* 31*/
            {
                -5, 55
            },
            /* 32*/
            {
                57, 56
            },
            /* 33*/
            {
                59, 58
            },
            /* 34*/
            {
                -2, 60
            },
            /* 35*/
            {
                62, 61
            },
            /* 36*/
            {
                64, 63
            },
            /* 37*/
            {
                66, 65
            },
            /* 38*/
            {
                68, 67
            },
            /* 39*/
            {
                70, 69
            },
            /* 40*/
            {
                72, 71
            },
            /* 41*/
            {
                73, -51
            },
            /* 42*/
            {
                75, 74
            },
            /* 43*/
            {
                77, 76
            },
            /* 44*/
            {
                -111, -101
            },
            /* 45*/
            {
                -97, -4
            },
            /* 46*/
            {
                79, 78
            },
            /* 47*/
            {
                80, -110
            },
            /* 48*/
            {
                -116, 81
            },
            /* 49*/
            {
                83, 82
            },
            /* 50*/
            {
                -255, 84
            },
            /* 51*/
            {
                86, 85
            },
            /* 52*/
            {
                88, 87
            },
            /* 53*/
            {
                90, 89
            },
            /* 54*/
            {
                -10, -15
            },
            /* 55*/
            {
                92, 91
            },
            /* 56*/
            {
                93, -21
            },
            /* 57*/
            {
                94, -117
            },
            /* 58*/
            {
                96, 95
            },
            /* 59*/
            {
                98, 97
            },
            /* 60*/
            {
                100, 99
            },
            /* 61*/
            {
                101, -114
            },
            /* 62*/
            {
                102, -105
            },
            /* 63*/
            {
                103, -26
            },
            /* 64*/
            {
                105, 104
            },
            /* 65*/
            {
                107, 106
            },
            /* 66*/
            {
                109, 108
            },
            /* 67*/
            {
                111, 110
            },
            /* 68*/
            {
                -3, 112
            },
            /* 69*/
            {
                -7, 113
            },
            /* 70*/
            {
                -131, 114
            },
            /* 71*/
            {
                -144, 115
            },
            /* 72*/
            {
                117, 116
            },
            /* 73*/
            {
                118, -20
            },
            /* 74*/
            {
                120, 119
            },
            /* 75*/
            {
                122, 121
            },
            /* 76*/
            {
                124, 123
            },
            /* 77*/
            {
                126, 125
            },
            /* 78*/
            {
                128, 127
            },
            /* 79*/
            {
                -100, 129
            },
            /* 80*/
            {
                -8, 130
            },
            /* 81*/
            {
                132, 131
            },
            /* 82*/
            {
                134, 133
            },
            /* 83*/
            {
                135, -120
            },
            /* 84*/
            {
                -31, 136
            },
            /* 85*/
            {
                138, 137
            },
            /* 86*/
            {
                -234, -109
            },
            /* 87*/
            {
                140, 139
            },
            /* 88*/
            {
                142, 141
            },
            /* 89*/
            {
                144, 143
            },
            /* 90*/
            {
                145, -112
            },
            /* 91*/
            {
                146, -19
            },
            /* 92*/
            {
                148, 147
            },
            /* 93*/
            {
                -66, 149
            },
            /* 94*/
            {
                -145, 150
            },
            /* 95*/
            {
                -65, -13
            },
            /* 96*/
            {
                152, 151
            },
            /* 97*/
            {
                154, 153
            },
            /* 98*/
            {
                155, -30
            },
            /* 99*/
            {
                157, 156
            },
            /* 100*/
            {
                158, -99
            },
            /* 101*/
            {
                160, 159
            },
            /* 102*/
            {
                162, 161
            },
            /* 103*/
            {
                163, -23
            },
            /* 104*/
            {
                164, -29
            },
            /* 105*/
            {
                165, -11
            },
            /* 106*/
            {
                -115, 166
            },
            /* 107*/
            {
                168, 167
            },
            /* 108*/
            {
                170, 169
            },
            /* 109*/
            {
                171, -16
            },
            /* 110*/
            {
                172, -34
            },
            /* 111*/
            {
                -132, 173
            },
            /* 112*/
            {
                -108, 174
            },
            /* 113*/
            {
                -22, 175
            },
            /* 114*/
            {
                -9, 176
            },
            /* 115*/
            {
                -84, 177
            },
            /* 116*/
            {
                -37, -17
            },
            /* 117*/
            {
                178, -28
            },
            /* 118*/
            {
                180, 179
            },
            /* 119*/
            {
                182, 181
            },
            /* 120*/
            {
                184, 183
            },
            /* 121*/
            {
                186, 185
            },
            /* 122*/
            {
                -104, 187
            },
            /* 123*/
            {
                -78, 188
            },
            /* 124*/
            {
                -61, 189
            },
            /* 125*/
            {
                -178, -79
            },
            /* 126*/
            {
                -134, -59
            },
            /* 127*/
            {
                -25, 190
            },
            /* 128*/
            {
                -18, -83
            },
            /* 129*/
            {
                -57, 191
            },
            /* 130*/
            {
                192, -67
            },
            /* 131*/
            {
                193, -98
            },
            /* 132*/
            {
                -68, -12
            },
            /* 133*/
            {
                195, 194
            },
            /* 134*/
            {
                -128, -55
            },
            /* 135*/
            {
                -50, -24
            },
            /* 136*/
            {
                196, -70
            },
            /* 137*/
            {
                -33, -94
            },
            /* 138*/
            {
                -129, 197
            },
            /* 139*/
            {
                198, -74
            },
            /* 140*/
            {
                199, -82
            },
            /* 141*/
            {
                -87, -56
            },
            /* 142*/
            {
                200, -44
            },
            /* 143*/
            {
                201, -248
            },
            /* 144*/
            {
                -81, -163
            },
            /* 145*/
            {
                -123, -52
            },
            /* 146*/
            {
                -113, 202
            },
            /* 147*/
            {
                -41, -48
            },
            /* 148*/
            {
                -40, -122
            },
            /* 149*/
            {
                -90, 203
            },
            /* 150*/
            {
                204, -54
            },
            /* 151*/
            {
                -192, -86
            },
            /* 152*/
            {
                206, 205
            },
            /* 153*/
            {
                -130, 207
            },
            /* 154*/
            {
                208, -53
            },
            /* 155*/
            {
                -45, -133
            },
            /* 156*/
            {
                210, 209
            },
            /* 157*/
            {
                -91, 211
            },
            /* 158*/
            {
                213, 212
            },
            /* 159*/
            {
                -88, -106
            },
            /* 160*/
            {
                215, 214
            },
            /* 161*/
            {
                217, 216
            },
            /* 162*/
            {
                -49, 218
            },
            /* 163*/
            {
                220, 219
            },
            /* 164*/
            {
                222, 221
            },
            /* 165*/
            {
                224, 223
            },
            /* 166*/
            {
                226, 225
            },
            /* 167*/
            {
                -102, 227
            },
            /* 168*/
            {
                228, -160
            },
            /* 169*/
            {
                229, -46
            },
            /* 170*/
            {
                230, -127
            },
            /* 171*/
            {
                231, -103
            },
            /* 172*/
            {
                233, 232
            },
            /* 173*/
            {
                234, -60
            },
            /* 174*/
            {
                -76, 235
            },
            /* 175*/
            {
                -121, 236
            },
            /* 176*/
            {
                -73, 237
            },
            /* 177*/
            {
                238, -149
            },
            /* 178*/
            {
                -107, 239
            },
            /* 179*/
            {
                240, -35
            },
            /* 180*/
            {
                -27, -71
            },
            /* 181*/
            {
                241, -69
            },
            /* 182*/
            {
                -77, -89
            },
            /* 183*/
            {
                -118, -62
            },
            /* 184*/
            {
                -85, -75
            },
            /* 185*/
            {
                -58, -72
            },
            /* 186*/
            {
                -80, -63
            },
            /* 187*/
            {
                -42, 242
            },
            /* 188*/
            {
                -157, -150
            },
            /* 189*/
            {
                -236, -139
            },
            /* 190*/
            {
                -243, -126
            },
            /* 191*/
            {
                -214, -142
            },
            /* 192*/
            {
                -206, -138
            },
            /* 193*/
            {
                -146, -240
            },
            /* 194*/
            {
                -147, -204
            },
            /* 195*/
            {
                -201, -152
            },
            /* 196*/
            {
                -207, -227
            },
            /* 197*/
            {
                -209, -154
            },
            /* 198*/
            {
                -254, -153
            },
            /* 199*/
            {
                -156, -176
            },
            /* 200*/
            {
                -210, -165
            },
            /* 201*/
            {
                -185, -172
            },
            /* 202*/
            {
                -170, -195
            },
            /* 203*/
            {
                -211, -232
            },
            /* 204*/
            {
                -239, -219
            },
            /* 205*/
            {
                -177, -200
            },
            /* 206*/
            {
                -212, -175
            },
            /* 207*/
            {
                -143, -244
            },
            /* 208*/
            {
                -171, -246
            },
            /* 209*/
            {
                -221, -203
            },
            /* 210*/
            {
                -181, -202
            },
            /* 211*/
            {
                -250, -173
            },
            /* 212*/
            {
                -164, -184
            },
            /* 213*/
            {
                -218, -193
            },
            /* 214*/
            {
                -220, -199
            },
            /* 215*/
            {
                -249, -190
            },
            /* 216*/
            {
                -217, -230
            },
            /* 217*/
            {
                -216, -169
            },
            /* 218*/
            {
                -197, -191
            },
            /* 219*/
            {
                243, -47
            },
            /* 220*/
            {
                245, 244
            },
            /* 221*/
            {
                247, 246
            },
            /* 222*/
            {
                -159, -148
            },
            /* 223*/
            {
                249, 248
            },
            /* 224*/
            {
                -93, -92
            },
            /* 225*/
            {
                -225, -96
            },
            /* 226*/
            {
                -95, -151
            },
            /* 227*/
            {
                251, 250
            },
            /* 228*/
            {
                252, -241
            },
            /* 229*/
            {
                -36, -161
            },
            /* 230*/
            {
                254, 253
            },
            /* 231*/
            {
                -39, -135
            },
            /* 232*/
            {
                -124, -187
            },
            /* 233*/
            {
                -251, 255
            },
            /* 234*/
            {
                -238, -162
            },
            /* 235*/
            {
                -38, -242
            },
            /* 236*/
            {
                -125, -43
            },
            /* 237*/
            {
                -253, -215
            },
            /* 238*/
            {
                -208, -140
            },
            /* 239*/
            {
                -235, -137
            },
            /* 240*/
            {
                -237, -158
            },
            /* 241*/
            {
                -205, -136
            },
            /* 242*/
            {
                -141, -155
            },
            /* 243*/
            {
                -229, -228
            },
            /* 244*/
            {
                -168, -213
            },
            /* 245*/
            {
                -194, -224
            },
            /* 246*/
            {
                -226, -196
            },
            /* 247*/
            {
                -233, -183
            },
            /* 248*/
            {
                -167, -231
            },
            /* 249*/
            {
                -189, -174
            },
            /* 250*/
            {
                -166, -252
            },
            /* 251*/
            {
                -222, -198
            },
            /* 252*/
            {
                -179, -188
            },
            /* 253*/
            {
                -182, -223
            },
            /* 254*/
            {
                -186, -180
            },
            /* 255*/
            {
                -247, -245
            }
        };

        #endregion

        public static bool DecompressChunk
        (
            Span<byte> src,
            ref int srcOffset,
            int srcLength,
            Span<byte> dest,
            int destOffset,
            out int destLength
        )
        {
            //Array.Clear(dest, destOffset, dest.Length - destOffset);
            destLength = 0;
            int end = srcOffset + srcLength;
            int node = 0;
            int destPos = destOffset;
            int bitNum = 8;

            while (srcOffset < end)
            {
                int leaf = (src[srcOffset] >> (bitNum - 1)) & 1;
                int leafValue = dec_tree[node, leaf];

                // all numbers below 1 (0..-256) are codewords
                // if the halt codeword has been found, skip this byte
                if (leafValue == -256)
                {
                    ++srcOffset;
                    destLength = destPos - destOffset;

                    return true;
                }

                if (leafValue < 1)
                {
                    dest[destPos] = (byte) -leafValue;
                    leafValue = 0;
                    ++destPos;
                }

                --bitNum;
                node = leafValue;

                /* if its the end of the byte, go to the next byte */
                if (bitNum < 1)
                {
                    bitNum = 8;
                    ++srcOffset;
                }

                // check to see if the current codeword has no end
                // if not, make it an incomplete byte
                if (srcOffset == srcLength)
                {
                    return false;
                }
            }

            destLength = destPos - destOffset;

            return false;
        }
    }
}