/* vssClient.h */

#ifndef __VSSCLIENT_H__
#define __VSSCLIENT_H__

#define DllExport __declspec(dllexport)
#define DllImport __declspec(dllimport)

#ifdef Dll2wayIsDllExport
#define Dll2way __declspec(dllexport)
#else
#define Dll2way __declspec(dllimport)
#endif

#include <stdlib.h>
#include <stdio.h>

#ifndef FOREVER
#include <limits.h>
typedef void *OBJ;
#ifndef TRUE
#define TRUE 1
#define FALSE 0
#endif
#define FOREVER ULONG_MAX
#endif

#define hNil (-1.f)
#define cchmm 5120
typedef struct mm
{
char fRetval;
char rgch[cchmm];
} mm;

extern int fdMidi; 
typedef struct
{
int cb;
char rgb[4];
} VSSMDevent;

#if defined(_LANGUAGE_C_PLUS_PLUS) || defined(__cplusplus)
#define DECL extern "C" Dll2way
#else
#define DECL extern Dll2way
#endif

DECL int AUDinit(const char *fileName);
DECL void AUDterminate(int fileHandle);
DECL void AUDEnableNoMessageGroupWarning(int fEnable); // added 4/19/99
DECL void AUDreset(void);
DECL void AUDupdate(int fileHandle, char *messageGroupName, int numFloats, float *floatArray);
DECL void AUDupdateTwo(int theFirst, int theSecond, char *messageGroupName, int numFloats, float *floatArray);
DECL void AUDupdateMany(int numHandles, int * handleArray, char *messageGroupName, int numFloats, float *floatArray);
DECL void AUDqueue(int, float*, float);
DECL void AUDflushQueue(int, char *, int fPreserveQueueData );
DECL void actorMessage(char* messagename);
DECL float actorMessageRetval(char* messagename);
DECL float actorGetReply(void);
DECL void AUDEnableNoMessageGroupWarning(int fEnable);
DECL float createActor(const char* actorType); 
DECL void deleteActor(const float actorHandle); 
DECL void setActorActive(const float actorHandle, const int active); 
DECL void dumpActor(const float actorHandle); 
DECL void dumpActors(); 
DECL float beginNote(const float hactor);
DECL void killSoundServer();
DECL void MsgsendObj(OBJ obj, struct sockaddr_in *paddr, mm* pmm);
DECL OBJ BgnMsgsend(char *szHostname, int channel);
DECL void setAckPrint(int flag);
DECL int BeginSoundServer(void);
DECL int BeginSoundServerAt(char * hostName);
DECL int SelectSoundServer(int serverHandle);
DECL void EndSoundServer(void);
DECL void EndAllSoundServers(void);
DECL int PingSoundServer(void);
DECL void Msgsend(struct sockaddr_in *paddr, mm* pmm);
DECL void MsgsendArgs1(struct sockaddr_in *paddr, mm* pmm, const char* msg, float z0);
DECL void MsgsendArgs2(struct sockaddr_in *paddr, mm* pmm, const char* msg, float z0, float z1);
DECL void clientMessageCall(char* Message);
DECL int WMsgFromSz(char *szMsg);
DECL const char* SzMsgFromW(int wMsg);
//DECL const char* GetVssLibVersion(void); 
//DECL const char* GetVssLibDate(void);
DECL VSSMDevent* GetMidiMessages(float* pcmsg, float hMidiActor);

#endif		/* __VSSCLIENT_H__ */
