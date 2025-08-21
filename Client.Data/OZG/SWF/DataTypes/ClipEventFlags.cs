namespace Client.Data.OZG.SWF.DataTypes;

public class ClipEventFlags
{
    /**
     * Key up event
     */
    public bool ClipEventKeyUp = false;

    /**
     * Key down event
     */
    public bool ClipEventKeyDown = false;

    /**
     * Mouse up event
     */
    public bool ClipEventMouseUp = false;

    /**
     * Mouse down event
     */
    public bool ClipEventMouseDown = false;

    /**
     * Mouse move event
     */
    public bool ClipEventMouseMove = false;

    /**
     * Clip unload event
     */
    public bool ClipEventUnload = false;

    /**
     * Frame event
     */
    public bool ClipEventEnterFrame = false;

    /**
     * Clip load event
     */
    public bool ClipEventLoad = false;

    /**
     * Mouse drag over event
     * @since SWF 6
     */
    public bool ClipEventDragOver = false;

    /**
     * Mouse rollout event
     * @since SWF 6
     */
    public bool ClipEventRollOut = false;

    /**
     * Mouse rollover event
     * @since SWF 6
     */
    public bool ClipEventRollOver = false;

    /**
     * Mouse release outside event
     * @since SWF 6
     */
    public bool ClipEventReleaseOutside = false;

    /**
     * Mouse release inside event
     * @since SWF 6
     */
    public bool ClipEventRelease = false;

    /**
     * Mouse press event
     * @since SWF 6
     */
    public bool ClipEventPress = false;

    /**
     * Initialize event
     * @since SWF 6
     */
    public bool ClipEventInitialize = false;

    /**
     * Data received event
     */
    public bool ClipEventData = false;

    /**
     * Reserved
     */
    public int Reserved;

    /**
     * Construct event
     * @since SWF 7
     */
    public bool ClipEventConstruct = false;

    /**
     * Key press event
     * @since SWF 6
     */
    public bool ClipEventKeyPress = false;

    /**
     * Mouse drag out event
     * @since SWF 6
     */
    public bool ClipEventDragOut = false;

    /**
     * Reserved
     */
    public int Reserved2;


     /**
     * Returns true if all events are false.
     *
     * @return True when all events are false
     */
    public bool isClear() {
        if (ClipEventKeyUp) {
            return false;
        }
        if (ClipEventKeyDown) {
            return false;
        }
        if (ClipEventMouseUp) {
            return false;
        }
        if (ClipEventMouseDown) {
            return false;
        }
        if (ClipEventMouseMove) {
            return false;
        }
        if (ClipEventUnload) {
            return false;
        }
        if (ClipEventEnterFrame) {
            return false;
        }
        if (ClipEventLoad) {
            return false;
        }
        if (ClipEventDragOver) {
            return false;
        }
        if (ClipEventRollOut) {
            return false;
        }
        if (ClipEventRollOver) {
            return false;
        }
        if (ClipEventReleaseOutside) {
            return false;
        }
        if (ClipEventRelease) {
            return false;
        }
        if (ClipEventPress) {
            return false;
        }
        if (ClipEventInitialize) {
            return false;
        }
        if (ClipEventData) {
            return false;
        }
        if (ClipEventConstruct) {
            return false;
        }
        if (ClipEventKeyPress) {
            return false;
        }
        if (ClipEventDragOut) {
            return false;
        }
        return true;
    }
}