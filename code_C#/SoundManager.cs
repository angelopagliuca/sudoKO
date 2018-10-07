using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager S;


	public GameObject BoardMusic;
	private AudioSource BoardMusicSound;

	public GameObject BoxingBell;
	private AudioSource BoxingBellSound;

	public GameObject Click;
	private AudioSource ClickSound;

    public GameObject Clock;
    private AudioSource ClockSound;

    public GameObject CompleteSquare;
    private AudioSource CompleteSquareSound;

    public GameObject Fight;
    private AudioSource FightSound;

	public GameObject FightMusic;
	private AudioSource FightMusicSound;

	public GameObject Hurt1;	
	private AudioSource Hurt1Sound;

    public GameObject Hurt2;
    private AudioSource Hurt2Sound;

    public GameObject Jump;
	private AudioSource JumpSound;

	public GameObject Kick;
	private AudioSource KickSound;

    public GameObject KO;
    private AudioSource KOSound;

    public GameObject MainMusic;
    private AudioSource MainMusicSound;

    public GameObject PlacingNumber;
    private AudioSource PlacingNumberSound;

    public GameObject Punch;
    private AudioSource PunchSound;

    public GameObject WrongTile;
    private AudioSource WrongTileSound;


    // Use this for initialization
    void Start () {
		S = this;
		
		BoardMusicSound = BoardMusic.GetComponent<AudioSource> ();
        BoxingBellSound = BoxingBell.GetComponent<AudioSource> ();
        ClickSound = Click.GetComponent<AudioSource> ();
        ClockSound = Clock.GetComponent<AudioSource>();
        CompleteSquareSound = CompleteSquare.GetComponent<AudioSource>();
        FightSound = Fight.GetComponent<AudioSource> ();
        FightMusicSound = FightMusic.GetComponent<AudioSource> ();
        Hurt1Sound = Hurt1.GetComponent<AudioSource> ();
        Hurt2Sound = Hurt2.GetComponent<AudioSource> ();
        JumpSound = Jump.GetComponent<AudioSource> ();
        KickSound = Kick.GetComponent<AudioSource> ();
        KOSound = KO.GetComponent<AudioSource>();
        MainMusicSound = MainMusic.GetComponent<AudioSource> ();
        PlacingNumberSound = PlacingNumber.GetComponent<AudioSource> ();
        PunchSound = Punch.GetComponent<AudioSource>();
        WrongTileSound = WrongTile.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    //void Update () {

    //}

    public void PlayBoardMusic() {
        BoardMusicSound.Play();
    }

    public void StopBoardMusic() {
        BoardMusicSound.Stop();
    }

    public void PlayBoxingBell() {
        BoxingBellSound.Play();
    }

    public void PlayClick() {
		ClickSound.Play();
	}

    public void PlayClock() { 
        ClockSound.Play();
    }

    public void StopClock() {
        ClockSound.Stop();
    }

    public void PlayCompleteSquare() { 
        CompleteSquareSound.Play();
    }

	public void PlayFightMusic() {
		FightMusicSound.Play();
	}

    public void PlayFightSound() {
        FightSound.Play();
    }

	public void StopFightMusic() {
		FightMusicSound.Stop();
	}

	public void PlayHurt1Sound() {
		Hurt1Sound.Play();
	}

    public void PlayHurt2Sound() {
        Hurt2Sound.Play();
    }

    public void PlayJumpSound() {
		JumpSound.Play();
	}

	public void PlayKickSound() {
	    KickSound.Play();
	}

    public void PlayKOSound(){
        KOSound.Play();
    }

    public void PlayMainMusic() {
        MainMusicSound.Play();
    }

    public void StopMainMusic() {
        MainMusicSound.Stop();
    }

	public void PlayPlacingNumberSound() {
        PlacingNumberSound.Play();
    }

    public void PlayPunchSound() {
        PunchSound.Play();
    }

    public void PlayWrongTileSound() {
        WrongTileSound.Play();
    }
}
